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


Dans Game development with Unity : Aucun paquet supplémentaires.

Dans Indiviual components :
- MSVC v142 x64/x86

## **Installation des paquets nécessaires au projet (déjà effectuée)**
### **Installation de M2MqttUnity**
Récupérez l'archive git à ce [lien](https://github.com/gpvigano/M2MqttUnity "M2MqttUnity"). Dézippez l'archive et copiez les dossiers M2Mqttt et Scripts dans vos Assets de Unity.

### **Installation de HoloLensARToolKit**
Récupérez l'archive git [ici](https://github.com/qian256/HoloLensARToolKit "HololensARToolKit"). Dézippez l'archive et copiez les dossiers ARToolKitUWP, Samples et StreamingAssets dans vos Assets de Unity.

# **Installations Ubuntu**
## **Installation de ROS Melodic**
Suivez les informations décrites [ici](http://wiki.ros.org/melodic/Installation/Ubuntu "Ros Melodic").

## **Installation de Mosquitto**
Il vous faut installer Mosquitto. Pour cela, vous pouvez utiliser la ligne de commande suivante : 
```
$ sudo apt-get install mosquitto
```  
Il faut également installer mosquitto_sub et mosquitto_pub qui permettent la communication : 
```
$ sudo apt-get install mosquitto-clients
```
Pour un fonctionnement optimal, il vous faut également créer un fichier dans /etc/mosquitto/config.d s'appellant default.conf, comme suit. Pour cela vous pouvez utiliser `vim`.
![image](https://github.com/cyrielle-clastres/MQTT_ROS_ArUcoBis/blob/main/Images/Config%20mosquitto.png)  
Pour lancer mosquitto, utilisez la commande suivante :
```
$ mosquitto -v -c /etc/mosquitto/config.d/default.conf
```
Pour arrêter mosquitto, utilisez la commande :
```
$ sudo service mosquitto stop
```

## **Installation de mqtt_bridge**
Il vous faudra également installer mqtt_bridge, utilisé dans la transmission des données de MQTT à ROS et inversement. Pour cela, télécharger le package [ici](https://github.com/groove-x/mqtt_bridge "mqtt_bridge").  
Dézippez le package dans `/catkin_ws/src`. Allez dans le fichier `dev-requirements.txt` et commentez la ligne suivante en ajoutant le symbole "#" devant:
```
git+https://github.com/RobotWebTools/rosbridge_suite.git#subdirectory=rosbridge_library
```
Installez les dépendances en exécutant les commandes suivantes dans le shell :
```
$ sudo apt install python3-pip
$ sudo apt install ros-noetic-rosbridge-library
$ pip3 install -r dev-requirements.txt
```
Il vous faudra ensuite changer le fichier de configuration `config/demo_params.yaml` comme suivant :

## **Installation de universal_robot**
Un paquet a été utilisé dans le but de communiquer et faire bouger le robot ur3e. Ce paquet a été trouvé sur Github et a été créé par ROS directement. Il est disponible [ici](https://github.com/ros-industrial/universal_robot "universal_robot"). Pour installer ce paquet, éxécutez les commandes suivantes :
```
$ cd /catkin_ws/src
$ git clone -b $melodic-devel https://github.com/ros-industrial/universal_robot.git
$ cd /catkin_ws
$ rosdep update
$ rosdep install --rosdistro $melodic --ignore-src --from-paths src
$ catkin_make
$ source /catkin_ws/devel/setup.bash
```

# **Déploiement de l'application**
## **Construire l'application sur Unity**
Avant de construire l'application il nous faut récupérer l'adresse IP de notre environnement Ubuntu. Pour cela il vous suffit d'aller dans votre environnement et de tapper la commande suivante : `hostname -I`. Récupérez cette adresse IP et copiez la dans l'objet M2MQTT, dans le champ Brocker Address de Unity.  
Vérifiez que la case Auto Connect est bien cochée.  
![image](https://github.com/cyrielle-clastres/MQTT_ROS_ArUcoBis/blob/main/Images/MQTT%20Settings.jpg)  
Puis, allez dans File > Build Settings > Universal Window Platform. Si vous n'avez pas installé le module, faites l'installation et rechargez le projet. Sélectionnez les paramètres ci-dessous et cliquez sur Switch Platform.  
Laissez le projet recharger. Une fois fait, sélectionnez "Add Open Scence" et cliquez sur Build. Sélectionnez un dossier approprié comme Builds et lancez la construction.

## **Construire et déployer l'application sur Visual Studio 2019**
Pour construire l'application et la déployer sur Visual Stuido, vous aurez besoin de l'adresse IP du casque HoloLens. Celle-ci est disponibe dans Paramètres > Mise à jour et sécurité > Pour les développeurs > Wi-Fi.  
![image](https://github.com/cyrielle-clastres/MQTT_ROS_ArUcoBis/blob/main/Images/Mode%20d%C3%A9veloppeur%20casque.jpg)
S'il s'agit de la première fois que le casque et l'ordinateur se connectent, cliquez sur Coupler. Un code apparaît. Restez sur ce menu jusqu'à ce qu'on vous le demande.
Choisir les modes Release et ARM64 pour construire l'application. Sélectionner également Ordinateur distant.  
![image](https://github.com/cyrielle-clastres/MQTT_ROS_ArUcoBis/blob/main/Images/ARM64.jpg)
Avant de lancer la contruction et le déploiement, allez dans Déboguer > Propriétés de débogage > Propriétés de configuration > Débogage. Mettez l'adresse IP du casque HoloLens dans le champ "Nom de l'ordinateur".  
![image](https://github.com/cyrielle-clastres/MQTT_ROS_ArUcoBis/blob/main/Images/Propri%C3%A9t%C3%A9s%20de%20d%C3%A9bogage.png)
![image](https://github.com/cyrielle-clastres/MQTT_ROS_ArUcoBis/blob/main/Images/D%C3%A9bogage%20IP.png)
Lancez la construction en allant dans Déboguer > Exécuter sans débogage. S'il s'agit de la première connexion entre le casque et l'ordinateur, une page va s'ouvrir demandant le code de couplage du casque. Entrez ce code.  
L'application va alors se déployer sur le casque et se lancer automatiquement.

# **Lancement de l'application**
## **Côté Ubuntu**
Avant de lancer l'application, il vous faudra lancer plusieurs commandes du côté de Ubuntu pour avoir toutes les fonctionnalités. Tout d'abord, il vous faut lancer Mosquitto comme ceci :
```
$ sudo service mosquitto stop
$ mosquitto -v - c /etc/mosquitto/config.d/default.conf
```
Il vous faut également lancer le noeud ROS qui gère la partie mqtt_bridge. Pour ce faire, lancez :
```
$ roslaunch hololens_ur3e demo.launch
```
Pour lancer la récupération des données du robot, lancez :
```
$ 
```
Pour afficher l'effecteur réel du robot, lancez :
```
$ rosrun ... logg_tool_V2.py
```
Pour déplacer le robot virtuel, lancez :
```
$ rosrun ur_kinematics calcul_position.pu
```

## **Côté Casque HoloLens2**
Une fois le déploiement fait, l'application se lance automatiquement. Faites en sorte de lancer le déploiement après les commandes marquées ci-dessus. Si jamais il a été fait avant, allez dans le menu Home du casque puis allez dans Paramètres > Applications > MQTT_ROS_ArUco > Options avancées > Terminer. L'application se ferme alors. Fermez la fenêtre correspondant aux paramètres et regardez autour de vous. Fermez toute fenêtre présente dans l'environnement.  
Lancez l'application en allant dans le menu Home > Toutes apps > MQTT_ROS_ArUco. L'application se lance alors.

# **Utilisation de l'application**
L'application lancée, allez regarder les ArUcos fixés à la verticale. Des trièdres apparaissent alors dessus en plus d'un central. Attendez de les voir sans bouger et bien alignés sur la feuille. Quand vous êtes satisfait(e) de leur emplacement, passez la main à travers le trièdre central. Les trièdres ne bougent alors plus.  
Regardez alors le coin inférieur gauche de la table. Un trièdre accompagné de boutons est apparu. Appuyez sur les boutons pour déplacer le trièdre en translation, l'axe rouge correspondant à x, l'axe vert à  y et l'axe bleu à z. Un second trièdre correspondant à ce déplacement apparaît alors. Déplacez le trièdre pour qu'il soit positionné dans le coin de la table, le cube parfaitement centré dans les 3 directions. Une fois satisfait(e) de ce déplacement, cliquez sur la croix visible à côté des boutons. Le premier trièdre disparaît alors, de même que les boutons, ne laissant que le trièdre que vous avez déplacé. Un trièdre apparaît au bout de l'effecteur du robot, de même qu'à sa base.  
Si les trièdre vous semblent mal positionnés, retournez à côté des ArUcos. Un bouton est visible, devant la table, légèrement à droite, à hauteur de buste. Cliquez sur ce bouton pour retirez ce que vous venez de placer. Vous pouvez alors recommencer les étapes ci-dessus.  
Lorque le placement vous satisfait, vous êtes alors prêt à choisir le mode qui vous intéresse.

# **Problèmes possibles** 
## **L'application charge à l'infini sur le casque**
C'est un problème de version Unity. Changez la version pour une autre et relancez.

## **L'application charge et se lance mais s'éteint instannément**
C'est un problème lié à la communication du casque vers l'ordinateur Ubuntu. Vérifiez votre adresse IP, votre connexion internet pour l'ordinateur et vérifiez également que vous avez bel et bien lancé les serveurs Mosquitto et ROS.

## **Le trièdre de l'effecteur ne s'affiche pas**
C'est un porblème lié aux commandes ROS. Une commande n'a pas dû être lancée. Vérifiez donc la partie Lancement de l'application > Côté Ubuntu.

## **Les boutons ne s'activent pas quand je les touche**
Pour activer un bouton, il faut passer son index à travers celui-ci dans le bon sens. C'est-à dire de l'extérieur vers l'intérieur. Si tout se passe bien, vous pouvez voir une petite boîte transparente à arètes blanches se comprimer lorsque vous la poussez.

# **Développement de l'application**
## **Package pour l'utilisation de MQTT**
Ce package a été trouvé sur Github à ce [lien](https://github.com/gpvigano/M2MqttUnity "M2MQTT"). Ce paquet n'a pas posé de problèmes dans son implémentation. Des fonctions ont été ajoutées pour corresopondre à nos besoin de communication.

## **Package pour détection de l'ArUco**
Ce package a été trouvé sur Github sur ce [lien](https://github.com/qian256/HoloLensARToolKit "HoloLensARToolkit"). Ce package n'a été implémenté que jusqu'à la version de Unity 2019.4.40.f1. Il a donc fallu l'Upgrade pour la version de Unity utilisée pour notre projet, la version 2020.3.11.f1.