/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>
namespace M2MqttUnity.Examples
{
    /// <summary>
    /// Script for testing M2MQTT with a Unity UI
    /// </summary>
    public class M2MqttUnityTest : M2MqttUnityClient
    {
        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
        [Header("User Interface")]
        public InputField consoleInputField;
        public Toggle encryptedToggle;
        public InputField addressInputField;
        public InputField portInputField;
        public Button connectButton;
        public Button disconnectButton;
        public Button testPublishButton;
        public Button clearButton;


        public PlacementCube placement_cube;
        public RobotVirtuel robot_virtuel;
        public Menu menu;
        public ValidationTrajectoire validation_trajectoire;
        public MatricesOutils matrices_outils;
        public Affichage affichage;
        public GameObject triedre_effecteur;
        public GameObject cube;

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;
        
        /*
         * PublishPosition est appelée lorsque le cube est déplacé.
         * Cette fonction permet de récupérer les coordonnées du cube dans l'espace indirect. Elle envoie la position et la rotation dans l'espace direct du robot
         * sous forme d'un message Pose de ROS au serveur MQTT présent sur Ubuntu.
         */
        public void PublishPosition()
        {
            if ((cube != null) && (placement_cube.fixe == 4))
            {
                // On calcule la position du cube dans le repère indirect
                Matrix4x4 m = Matrix4x4.TRS(cube.transform.position, cube.transform.rotation, new Vector3(1, 1, 1));
                Matrix4x4 cube_robot = placement_cube.mat_monde_robot * m;

                // On calcule la position du cube dans le repère direct de la table
                Vector3 Point = new Vector3(cube_robot[0,3], cube_robot[2,3], cube_robot[1,3]);
                Quaternion Quaternion = new Quaternion(-cube_robot.rotation.x, -cube_robot.rotation.z, -cube_robot.rotation.y, cube_robot.rotation.w);

                // On envoie la position et la rotation du cube dans le repère direct à MQTT qui fera la transmission à ROS
                String pos = JsonUtility.ToJson(Point);
                String rot = JsonUtility.ToJson(Quaternion);
                String final = "{\"position\":" + pos + " , \"orientation\":" + rot + "}";
                client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(final)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        /*
         * PublishTriedrePosition est appelée lorsque le trièdre associé au robot virtuel est déplacé.
         * Cette fonction permet de récupérer les coordonnées du trièdre dans l'espace indirect. Elle envoie la position et la rotation dans l'espace direct du robot
         * sous forme d'un message Pose de ROS au serveur MQTT présent sur Ubuntu.
         * Elle envoie également les positions des joints du robot virtuel dans le but de trouver la solution pour la position du trièdre la plus proche de la position
         * actuelle.
         */
        public void PublishTriedrePosition()
        {
            if ((placement_cube.fixe == 4) && (robot_virtuel.SetJoints == true) && (robot_virtuel.SetTriedre == true) && (menu.emplacement == 1))
            {
                // On envoie la position actuelle des joints
                JointState joint = new JointState();
                joint.position = robot_virtuel.GetPosition();
                joint.velocity = new float[6];
                joint.effort = new float[6];
                String position_robot = JsonUtility.ToJson(joint);
                client.Publish("joint_state_virtual", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(position_robot)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

                // On calcule la position du triedre dans le repère indirect
                Matrix4x4 m = Matrix4x4.TRS(triedre_effecteur.transform.position, triedre_effecteur.transform.rotation, new Vector3(1, 1, 1));
                Matrix4x4 triedre_robot = placement_cube.mat_monde_robot * m;
                // Pour le calcul du modèle inverse qui se fait dans la base link avec le flange
                if(menu.outil == 1)
                {
                    triedre_robot = triedre_robot * matrices_outils.mat_feutre_tool0;
                }
                if (menu.outil == 2)
                {
                    triedre_robot = triedre_robot * matrices_outils.mat_cercle20_tool0;
                }
                if (menu.outil == 3)
                {
                    triedre_robot = triedre_robot * matrices_outils.mat_cercle40_tool0;
                }
                if (menu.outil == 4)
                {
                    triedre_robot = triedre_robot * matrices_outils.mat_cercle50_tool0;
                }
                triedre_robot = triedre_robot * matrices_outils.mat_tool0_flange;
                triedre_robot = matrices_outils.mat_base_link_base * triedre_robot;

                // On calcule la position du trièdre dans le repère direct de la table
                Vector3 Point = new Vector3(triedre_robot[0, 3], triedre_robot[2, 3], triedre_robot[1, 3]);
                Quaternion Quaternion = new Quaternion(-triedre_robot.rotation.x, -triedre_robot.rotation.z, -triedre_robot.rotation.y, triedre_robot.rotation.w);

                // On envoie la position et la rotation du trièdre dans le repère direct à MQTT qui fera la transmission à ROS
                String pos = JsonUtility.ToJson(Point);
                String rot = JsonUtility.ToJson(Quaternion);
                String final = "{\"position\":" + pos + " , \"orientation\":" + rot + "}";
                client.Publish("robot_pose", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(final)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        public void PublishTrajectoire()
        {
            if(robot_virtuel.TrajectoireFinie == true)
            {
                robot_virtuel.TrajectoireEnCours = true;
                String trajectoire_robot = JsonUtility.ToJson(robot_virtuel.trajectoire);
                client.Publish("trajectoire_robot", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(trajectoire_robot)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                robot_virtuel.SetJoints = true;
                robot_virtuel.SetTriedre = true;
            }
        }

        public void PublishOutil()
        {
            String choix_outil = menu.nom_outil[menu.outil];
            String final = "{\"data\":" + "\"" + choix_outil + "\"" + "}";
            client.Publish("outil", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(final)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void TestPublish()
        {
            client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            AddUiMessage("Test message published.");
        }

        public void SetBrokerAddress(string brokerAddress)
        {
            if (addressInputField && !updateUI)
            {
                this.brokerAddress = brokerAddress;
            }
        }

        public void SetBrokerPort(string brokerPort)
        {
            if (portInputField && !updateUI)
            {
                int.TryParse(brokerPort, out this.brokerPort);
            }
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        public void SetUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text = msg;
                updateUI = true;
            }
        }

        public void AddUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text += msg + "\n";
                updateUI = true;
            }
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            if (autoTest)
            {
                TestPublish();
            }
        }

        /*
         * SubscribeTopics est appelée lorsque le Casque se connecte à Ubuntu.
         * Cette fonction permet de récupérer les messages postés sur les différents topics.
         */
        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "position/mqtt" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "position_robot" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "position_robot_virtuel_casque" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "trajectoire_finie" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        /*
         * UnsubscribeTopics est appelée lorsque le Casque se déconnecte à Ubuntu.
         * Cette fonction permet d'arrêter de récupérer les messages postés sur les différents topics.
         */
        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "position/mqtt" });
            client.Unsubscribe(new string[] { "position_robot" });
            client.Unsubscribe(new string[] { "position_robot_virtuel_casque" });
            client.Unsubscribe(new string[] { "trajectoire_finie" });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            AddUiMessage("CONNECTION LOST!");
        }

        private void UpdateUI()
        {
            if (client == null)
            {
                if (connectButton != null)
                {
                    connectButton.interactable = true;
                    disconnectButton.interactable = false;
                    testPublishButton.interactable = false;
                }
            }
            else
            {
                if (testPublishButton != null)
                {
                    testPublishButton.interactable = client.IsConnected;
                }
                if (disconnectButton != null)
                {
                    disconnectButton.interactable = client.IsConnected;
                }
                if (connectButton != null)
                {
                    connectButton.interactable = !client.IsConnected;
                }
            }
            if (addressInputField != null && connectButton != null)
            {
                addressInputField.interactable = connectButton.interactable;
                addressInputField.text = brokerAddress;
            }
            if (portInputField != null && connectButton != null)
            {
                portInputField.interactable = connectButton.interactable;
                portInputField.text = brokerPort.ToString();
            }
            if (encryptedToggle != null && connectButton != null)
            {
                encryptedToggle.interactable = connectButton.interactable;
                encryptedToggle.isOn = isEncrypted;
            }
            if (clearButton != null && connectButton != null)
            {
                clearButton.interactable = connectButton.interactable;
            }
            updateUI = false;
        }

        protected override void Start()
        {
            SetUiMessage("Ready.");
            updateUI = true;
            base.Start();
        }

        /*
         * DecodeMessage est appelée à chaque frame.
         * Cette fonction permet de décoder les messages reçus sur les différents topics auxquels on est abonné.
         * Elle permet de récupérer la position de l'effecteur, la position des joints du robot et également les positions calculées pour le robot virtuel.
         */
        protected override void DecodeMessage(string topic, byte[] message)
        {
            // On récupère le message et le topic sur lequel il a été publié
            string msg = System.Text.Encoding.UTF8.GetString(message);
            StoreMessage(msg);
            if (topic == "position/mqtt")
            {
                // On transforme le message en un objet pour récupérer tous les champs
                PosRot PositionAndRotation = JsonUtility.FromJson<PosRot>(msg);
                if ((triedre_effecteur != null) && (placement_cube.fixe == 4))
                {
                    menu.count++;

                    // On récupère les coordonnées envoyées par ROS et on les mets dans le repère indirect de la table
                    PositionAndRotation.position = new Vector3(PositionAndRotation.position.x, PositionAndRotation.position.z, PositionAndRotation.position.y);
                    PositionAndRotation.orientation = new Quaternion(-PositionAndRotation.orientation.x, -PositionAndRotation.orientation.z, -PositionAndRotation.orientation.y, PositionAndRotation.orientation.w);

                    // On calcule les coordonnées indirectes dans le repère monde
                    Matrix4x4 m = Matrix4x4.TRS(PositionAndRotation.position, PositionAndRotation.orientation, new Vector3(1, 1, 1));
                    Matrix4x4 triedre_monde = placement_cube.mat_robot_monde * m;

                    // On donne les coordonnées à l'objet
                    if (menu.emplacement == 2)
                    {
                        triedre_effecteur.transform.position = new Vector3(triedre_monde[0, 3], triedre_monde[1, 3], triedre_monde[2, 3]);
                        triedre_effecteur.transform.rotation = new Quaternion(triedre_monde.rotation.x, triedre_monde.rotation.y, triedre_monde.rotation.z, triedre_monde.rotation.w);
                    }
                        
                    // On donne également les coordonnées au trièdre associé au robot virtuel si cela n'a pas déjà été fait
                    if((((robot_virtuel.SetTriedre == false) && (robot_virtuel.TrajectoireEnCours == false) && (menu.count == 10)) || ((robot_virtuel.SetTriedre == true) && (robot_virtuel.TrajectoireEnCours == true))) && (menu.emplacement == 1))
                    {
                        triedre_effecteur.transform.position = new Vector3(triedre_monde[0, 3], triedre_monde[1, 3], triedre_monde[2, 3]);
                        triedre_effecteur.transform.rotation = new Quaternion(triedre_monde.rotation.x, triedre_monde.rotation.y, triedre_monde.rotation.z, triedre_monde.rotation.w);
                        // Une fois ce placement fait, on ne veut pas que cela soit possible à nouveau, on met la variable à vrai. Sinon, le robot trace la trajectoire et on le laisse faire.
                        robot_virtuel.SetTriedre = true;
                    }
                }
            }

            // On récupère la position des Joints du robot réel et on applique ces valeurs au robot virtuel
            if ((topic == "position_robot") && ((placement_cube.fixe == 4) || (placement_cube.fixe == 1)))
            {
                // On transforme le message en objet pour récupérer tous les champs
                JointState Joint_State = JsonUtility.FromJson<JointState>(msg);

                // On donne les positions au robot virtuel se déplaçant en virtuel si cela n'a pas déjà été fait
                if (((robot_virtuel.SetJoints == false) && (robot_virtuel.TrajectoireEnCours == false)) || ((robot_virtuel.SetJoints == true) && (robot_virtuel.TrajectoireEnCours == true)))
                {
                    robot_virtuel.UpdatePosition(Joint_State.position);

                    if(menu.emplacement == 1)
                    {
                        // Une fois ce placement fait, on ne veut pas que cela soit possible à nouveau, on met la variable à vrai.
                        robot_virtuel.SetJoints = true;
                    }   
                }
            }

            // On récupère les positions des Joints calculées selon la position de l'effecteur demandée
            if (topic == "position_robot_virtuel_casque")
            {
                // On crée la trajectoire de points
                if (robot_virtuel.TrajectoireFinie == false)
                {
                    // On transforme le message en objet pour récupérer tous les champs
                    JointState Joint_State = JsonUtility.FromJson<JointState>(msg);

                    // On donne les positions calculées au robot virtuel
                    robot_virtuel.UpdatePosition(Joint_State.position);
                    if (validation_trajectoire.SetPremierPoint == true)
                    {
                        JointTrajectoryPoint point = new JointTrajectoryPoint();
                        point.positions = Joint_State.position;
                        robot_virtuel.point.Add(point);
                    }
                }
            }

            // La trajectoire est finie, on peut à nouveau déplacer le trièdre
            if (topic == "trajectoire_finie")
            {
                if(robot_virtuel.TrajectoireEnCours == true)
                {
                    robot_virtuel.point.Reverse();
                    robot_virtuel.trajectoire.points = null;
                    robot_virtuel.TrajectoireFinie = false;
                    placement_cube.fixe = 5;
                    affichage.TrajectoireEnvers();
                }
            }
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            AddUiMessage("Received: " + msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            if (updateUI)
            {
                UpdateUI();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            if (autoTest)
            {
                autoConnect = true;
            }
        }
    }
}
