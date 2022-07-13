# **HoloLens2WithUR3**
## **Auteur**
Cyrielle CLASTRES

# **Description**
Ce projet permet de coupler un robot UR3e virtuel sur un casque HoloLens2 à un robot réel UR3e via différents moyens. Le casque HoloLens2 envoie des données à un ordinateur Ubuntu via MQTT. Celui-ci transmet les informations à ROS, qui les utilise pour communiquer avec le robot.

# **Installations Windows**
## **Installation de Unity**
Il faut tout d'abord installer Unity Hub disponible [ici](https://unity3d.com/fr/get-unity/download "Unity Hub").  
Une fois installé, il vous faudra télécharger Unity.2020.3.11.f1 disponible [ici](https://unity3d.com/get-unity/download/archive "Unity.2020.3.11.f1"). N'oubliez pas d'installer Universal Windows Platform également.  

## **Installation de Visual Studio 2019**
Il vous faudra également disposer de VisualStudio 2019 disponible [ici](https://visualstudio.microsoft.com/fr/vs/older-downloads/ "VisualStudio 2019").  

Ouvrez Visual Studio Installer et installez les packages suivants :  
Dans Desktop development with C++ :  
- MSVC v142 x64/x86
- Windows 10 SDK (10.0.19041.0)
- C++ profiling tools
- IntelliCode
- C++ AddressSanitizer
- MSVC v142 AMR64
- Windows 10 SDK (10.0.18362.0)
- MSVC v141 x64/x86


Dans Universal Windows Platform Development :
- Intellicode
- USB Device COnnectivity
- C++ (v142) UWP
- C++ (v141) UWP
- Graphics dbugger
-  Windows 10 SDK (10.0.18362.0) (même version que plus haut)


Dans Game development with C++ :
- C++ profiling tools
- C++ AddressSaanitizer
- Windows 10 DSK (10.0.19041.0)
- Intellicode
- Windows 10 SDK (10.0.18362.0) (même version que plus haut)


Dans Game development with Unity : Rien de plus.

Dans Indiviual components :
- MSVC v142 x64/x86

## **Installation des paquets nécessaires au projet**
### **Installation de M2MqttUnity**
Récupérez l'archive git à ce [lien](https://github.com/gpvigano/M2MqttUnity "M2MqttUnity"). Dézippez l'archive et copiez les dossiers M2Mqttt et Scripts dans vos Assets de Unity.

### **Installation de HoloLensARToolKit**
Récupérez l'archive git [ici](https://github.com/qian256/HoloLensARToolKit "HololensARToolKit"). Dézippez l'archive et copiez les dossiers ARToolKitUWP, Samples et StreamingAssets dans vos Assets de Unity.

# **Installations Ubuntu**
## **Installation de ROS Melodic**
Suivez les informations décrites [ici](http://wiki.ros.org/melodic/Installation/Ubuntu "Ros Melodic").

## **Installation de Mosquitto**
Il vous faut installer Mosquitto. Pour cela, vous pouvez utiliser la ligne de commande suivante : `sudo apt-get install mosquitto`.  
Il faut également installer mosquitto_sub et mosquitto_pub qui permettent la communication : `sudo apt-get install mosquitto-clients`.  
Pour un fonctionnement optimal, il vous faut également créer un fichier dans /etc/mosquitto/config.d s'appellant default.conf, comme suit. Pour cela vous pouvez utiliser `vim`.
![image](../../Unity/HoloLens2WithUR3/Images/Config%20mosquitto.png)  
Pour lancer mosquitto, utilisez la commande suivante : `mosquitto -v -c /etc/mosquitto/config.d/default.conf`.  
Pour arrêter mosquitto, utilisez la commande : `sudo service mosquitto stop`.

## **Installation de mqtt_bridge**
Il vous faudra également installer mqtt_bridge, utilisé dans la transmission des données de MQTT à ROS et inversement. Pour cela, télécharger le package [ici](https://github.com/groove-x/mqtt_bridge "mqtt_bridge").  
Dézippez le package. Allez dans le fichier `dev-requirements.txt` et commentez la ligne suivante : `git+https://github.com/RobotWebTools/rosbridge_suite.git#subdirectory=rosbridge_library`. Installez les dépendances en exécutant les commandes suivantes dans le shell :
```
$ sudo apt install python3-pip
$ sudo apt install ros-noetic-rosbridge-library
$ pip3 install -r dev-requirements.txt
```
Il vous faudra ensuite changer le fichier de configuration `config/demo_params.yaml` comme suivant :


# **Déploiement de l'application**
## **Construire l'application sur Unity**
Avant de construire l'application il nous faut récupérer l'adresse IP de notre environnement Ubuntu. Pour cela il vous suffit d'aller dans votre environnement et de tapper la commande suivante : `hostname -I`. Récupérez cette adresse IP et copiez la dans l'objet M2MQTT, dans le champ Brocker Address de Unity.  
Vérifiez que la case Auto Connect est bien cochée.  
![image](../HoloLens2WithUR3/Images/MQTT%20Settings.jpg)  
Puis, allez dans File > Build Settings > Universal Window Platform. Si vous n'avez pas installé le module, faites l'installation et rechargez le projet. Sélectionnez les paramètres ci-dessous et cliquez sur Switch Platform.  
Laissez le projet recharger et, une fois fait, cliquez sur Build. Sélectionnez un dossier approprié comme Buils et lancez la construction.

## **Construire et déployer l'application sur Visual Studio 2019**
Pour construire l'application et la déployer sur Visual Stuido, vous aurez besoin de l'adresse IP du casque HoloLens. Celle-ci est disponibe dans Paramètres > Mise à jour et sécurité > Pour les développeurs > Wi-Fi.  
![image](../HoloLens2WithUR3/Images/Mode%20d%C3%A9veloppeur%20casque.jpg)
S'il s'agit de la première fois que le casque et l'ordinateur se connectent, cliquez sur Coupler. Un code apparaît. Restez sur ce menu jusqu'à ce qu'on vous le demande.
Choisir les modes Release et ARM64 pour construire l'application. Sélectionner également Ordinateur distant.  
![image](../HoloLens2WithUR3/Images/ARM64.jpg)
Avant de lancer la contruction et le déploiement, allez dans Déboguer > Propriétés de débogage > Propriétés de configuration > Débogage. Mettez l'adresse IP du casque HoloLens dans le champ "Nom de l'ordinateur".  
![image](../HoloLens2WithUR3/Images/Propri%C3%A9t%C3%A9s%20de%20d%C3%A9bogage.png)
![image](../HoloLens2WithUR3/Images/D%C3%A9bogage%20IP.png)
Lancez la construction en allant dans Déboguer > Exécuter sans débogage. S'il s'agit de la première connexion entre le casque et l'ordinateur, une page va s'ouvrir demandant le code de couplage du casque.

# **Problèmes possibles** 
## **L'application charge à l'infini sur le casque**
C'est un problème de version Unity. Changez la version pour une autre et relancez.

## **L'application charge et se lance mais s'éteint instannément**
C'est un problème lié à la communication du casque vers l'ordinateur Ubuntu. Vérifiez votre adresse IP, votre connexion internet pour l'ordinateur et vérifiez également que vous avez bel et bien lancé les serveurs Mosquitto et ROS.

# **Développement de l'application**
## **Package pour l'utilisation de MQTT**

## **Package pour détection de l'ArUco**
Ce package a été trouvé sur Github sur ce [lien](https://github.com/qian256/HoloLensARToolKit "HoloLensARToolkit"). Ce package n'a été implémenté que jusqu'à la version de Unity 2019.4.40.f1. Il a donc fallu l'Upgrade pour la version de Unity utilisée pour notre projet, la version 2020.3.11.f1.  
Pour utiliser ce paquet, copiez les dossiers ... dans les Assets.