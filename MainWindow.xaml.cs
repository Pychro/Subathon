
//Created by Pychro

























using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace SubathonWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int seconds = 0;
        int minutes = 0;
        int hours = 0;
        int days = 0;
        String timeLeftConverter;

        int elapsedSeconds = 0;
        int elapsedMinutes = 0;
        int elapsedHours = 0;
        int elapsedDays = 0;

        int startSeconds = 0;
        int startMinutes = 0;
        int startHours = 0;
        int startDays = 0;

        int maxSeconds = 0;
        int maxMinutes = 0;
        int maxHours = 0;
        int maxDays = 0;

        string displaySeconds;
        string displayMinutes;
        string displayHours;
        string displayDays;

        int tier1Num = 0;
        int tier2Num = 0;
        int tier3Num = 0;
        int primeNum = 0;

        DateTime dateNow;
        DateTime dateEnd;

        TimeSpan timeLeft;

        bool isStarted = false;
        bool isStreamerSelected;


        int overallSeconds;
        int endSeconds = 1;

        public String streamerUser;

        TwitchClient client;

        Display display = new Display();
        public int timerBarValue;
        public String timerBarText;

        int SubTypeChoice;
        int tier1TypeChoice;
        int tier2TypeChoice;
        int tier3TypeChoice;
        int primeTypeChoice;

        bool pychConnected = false;

        public System.Timers.Timer timer1 = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            startTimer();
        }

        private void startTimer()
        {
            timer1.Elapsed += timer1_Tick;
            timer1.Interval = 1000;

        }

        private void updateTime()
        {

            if (days > 0)
            {
                timeLeftConverter = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";
                


            }

            else if (days < 1 && hours > 0)
            {
                timeLeftConverter = hours + " hours " + minutes + " minutes " + seconds + " seconds";
                

            }

            else if (hours < 1 && minutes > 0)
            {
                timeLeftConverter = minutes + " minutes " + seconds + " seconds";
                
            }

            else if (minutes < 1 && seconds > 0)
            {
                timeLeftConverter = seconds + " seconds";
                

            }
            else
            {
                timeLeftConverter = "Estimating...";
                
            }
            updateContent(timeLeftLabel, timeLeftConverter);
            
            

            if (elapsedDays > 0)
            {
                updateContent(elapsedTimeLabel, elapsedDays + " days " + elapsedHours + " hours " + elapsedMinutes + " minutes " + elapsedSeconds + " seconds");
            }

            else if (elapsedDays < 1 && elapsedHours > 0)
            {
                updateContent(elapsedTimeLabel, elapsedHours + " hours " + elapsedMinutes + " minutes " + elapsedSeconds + " seconds");
                
            }

            else if (elapsedHours < 1 && elapsedMinutes > 0)
            {
                updateContent(elapsedTimeLabel, elapsedMinutes + " minutes " + elapsedSeconds + " seconds");
            }

            else if (elapsedMinutes < 1 && elapsedSeconds > 0)
            {
                updateContent(elapsedTimeLabel, elapsedSeconds + " seconds");
            }

            else
            {
                
                    updateContent(elapsedTimeLabel, "Estimating...");
                
                
            }

            dateNow = DateTime.Now;
            timeLeft = new TimeSpan(days, hours, minutes, seconds);
            dateEnd = dateNow.Add(timeLeft);


            updateContent(estimatedEndLabel, dateEnd.ToString());
            


        }

        private void updateContent(Label content, string input)
        {
            this.Dispatcher.Invoke(() =>
            {
                content.Content = input;
            });
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            overallSeconds++;
            seconds--;
            if (seconds > 0)
            {
                updateTime();
            }
            else
            {
                seconds = 59;
                minutes--;
                if (minutes >= 0)
                {

                    updateTime();
                }
                else
                {
                    minutes = 59;
                    hours--;
                    if (hours < 0)
                    {

                        hours = 23;
                        days--;
                        updateTime();
                    }
                    else
                    {
                        updateTime();
                    }
                }
            }
            elapsedSeconds++;
            if (elapsedSeconds < 60)
            {
                updateTime();
            }
            else
            {
                elapsedMinutes++;
                elapsedSeconds = 0;
                if (elapsedMinutes < 60)
                {
                    updateTime();
                }
                else
                {
                    elapsedMinutes = 0;
                    elapsedHours++;
                    if (elapsedHours > 23)
                    {
                        elapsedDays++;
                        elapsedHours = 0;
                        updateTime();
                    }
                    else
                    {
                        updateTime();
                    }
                }
            }
            endSeconds = startSeconds + (startMinutes * 60) + (startHours * 60 * 60) + (startDays * 24 * 60 * 60);
            

            display.setMax(endSeconds);
            display.setText(timerText());
            display.setValue(overallSeconds);
        }

        private string timerText()
        {
            if(days > 9)
            {
                displayDays = days.ToString();
            }
            else
            {
                displayDays = "0" + days.ToString();
            }

            if (hours > 9)
            {
                displayHours = hours.ToString();
            }
            else
            {
                displayHours = "0" + hours.ToString();
            }

            if (minutes > 9)
            {
                displayMinutes = minutes.ToString();
            }
            else
            {
                displayMinutes = "0" + minutes.ToString();
            }

            if (seconds > 9)
            {
                displaySeconds = seconds.ToString();
            }
            else
            {
                displaySeconds = "0" + seconds.ToString();
            }
            return displayDays + ":" + displayHours + ":" + displayMinutes + ":" + displaySeconds;
        }


        private void startDaysNum_ValueChanged(object sender, EventArgs e)
        {
            startDays = ((int)startDaysNum.Value);
        }
        
        

        private void startSecondsNum_ValueChanged(object sender, EventArgs e)
        {
            startSeconds = ((int)startSecondsNum.Value);
        }

        private void startHoursNum_ValueChanged(object sender, EventArgs e)
        {
            startHours = ((int)startHoursNum.Value);
        }

        private void startMinutesNum_ValueChanged(object sender, EventArgs e)
        {
            startMinutes = ((int)startMinutesNum.Value);
        }

        private void maxDaysNum_ValueChanged(object sender, EventArgs e)
        {
            maxDays = ((int)maxDaysNum.Value);
        }

        private void maxHoursNum_ValueChanged(object sender, EventArgs e)
        {
            maxHours = ((int)maxHoursNum.Value);
        }

        private void maxMinutesNum_ValueChanged(object sender, EventArgs e)
        {
            maxMinutes = ((int)maxMinutesNum.Value);
        }

        private void maxSecondsNum_ValueChanged(object sender, EventArgs e)
        {
            maxSeconds = ((int)maxSecondsNum.Value);
        }

        private void tier1NumCounter_ValueChanged(object sender, EventArgs e)
        {
            tier1Num = ((int)tier1NumCounter.Value);
        }

        private void tier2NumCounter_ValueChanged(object sender, EventArgs e)
        {
            tier2Num = ((int)tier2NumCounter.Value);
        }

        private void tier3NumCounter_ValueChanged(object sender, EventArgs e)
        {
            tier3Num = ((int)tier3NumCounter.Value);
        }
        private void primeNumCounter_ValueChanged(object sender, EventArgs e)
        {
            primeNum = ((int)primeNumCounter.Value);
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subType.SelectedIndex == 0)
            {
                timePerSubGroup.Visibility = Visibility.Visible;
                rouletteGroup.Visibility = Visibility.Hidden;
            }
            else
            {
                timePerSubGroup.Visibility = Visibility.Hidden;
                rouletteGroup.Visibility = Visibility.Visible;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            streamerUser = streamerUserText.Text;
            isStreamerSelected = true;
        }



        public void subAdder(string tier)
        {
            if (tier == SubscriptionPlan.Tier1.ToString())
            {
                Console.WriteLine("tier1 confirmed");
                if (SubTypeChoice == 0)
                {
                    Console.WriteLine("Made it to time per sub");
                    if (tier1TypeChoice == 0)
                    {
                        days = days + tier1Num;
                        Console.WriteLine("made it to days");
                    }
                    else if (tier1TypeChoice == 1)
                    {
                        hours = hours + tier1Num;
                        Console.WriteLine("made it to hours");
                    }
                    else if (tier1TypeChoice == 2)
                    {
                        minutes = minutes + tier1Num;
                    }
                    else if (tier1TypeChoice == 3)
                    {
                        seconds = seconds + tier1Num;
                    }
                    else
                    {
                        Console.WriteLine("Sent to else");
                    }
                }
                else
                {

                }
            }
            else if (tier == SubscriptionPlan.Prime.ToString())
            {
                Console.WriteLine("prime confirmed");
                if (SubTypeChoice == 0)
                {
                    Console.WriteLine("Made it to time per sub");
                    if (primeTypeChoice == 0)
                    {
                        days = days + tier1Num;
                        Console.WriteLine("made it to days");
                    }
                    else if (primeTypeChoice == 1)
                    {
                        hours = hours + tier1Num;
                        Console.WriteLine("made it to hours");
                    }
                    else if (primeTypeChoice == 2)
                    {
                        minutes = minutes + tier1Num;
                    }
                    else if (primeTypeChoice == 3)
                    {
                        seconds = seconds + tier1Num;
                    }
                    else
                    {
                        Console.WriteLine("Sent to else");
                    }
                }

                else
                {
                    
                }
            }
            else if (tier == SubscriptionPlan.Tier2.ToString())
            {
                Console.WriteLine("tier2 confirmed");
                if (SubTypeChoice == 0)
                {
                    Console.WriteLine("Made it to time per sub");
                    if (tier2TypeChoice == 0)
                    {
                        days = days + tier2Num;
                        Console.WriteLine("made it to days");
                    }
                    else if (tier2TypeChoice == 1)
                    {
                        hours = hours + tier2Num;
                        Console.WriteLine("made it to hours");
                    }
                    else if (tier2TypeChoice == 2)
                    {
                        minutes = minutes + tier2Num;
                    }
                    else if (tier2TypeChoice == 3)
                    {
                        seconds = seconds + tier2Num;
                    }
                    else
                    {
                        Console.WriteLine("Sent to else");
                    }
                }
                else
                {

                }
            }
            else if (tier == SubscriptionPlan.Tier3.ToString())
            {
                Console.WriteLine("tier3 confirmed");
                if (SubTypeChoice == 0)
                {
                    Console.WriteLine("Made it to time per sub");
                    if (tier3TypeChoice == 0)
                    {
                        days = days + tier3Num;
                        Console.WriteLine("made it to days");
                    }
                    else if (tier3TypeChoice == 1)
                    {
                        hours = hours + tier3Num;
                        Console.WriteLine("made it to hours");
                    }
                    else if (tier3TypeChoice == 2)
                    {
                        minutes = minutes + tier3Num;
                    }
                    else if (tier3TypeChoice == 3)
                    {
                        seconds = seconds + tier3Num;
                    }
                    else
                    {
                        Console.WriteLine("Sent to else");
                    }
                }
                else
                {

                }
            }
        }

        private void wakePychbot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("Pychbot", "oauth:n7qug1nikde354c5asejmlv7wl7sh4");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, streamerUser);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnReSubscriber += Client_OnReSubscriber;
            client.OnGiftedSubscription += Client_OnGiftedSubscription;


            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }


        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {

            client.SendMessage(e.Channel, "Pychbot has connected");
            pychConnected = true;
        }



        public void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            subAdder(e.Subscriber.SubscriptionPlan.ToString());

            Console.WriteLine("New sub " + e.Subscriber.SubscriptionPlan.ToString());
        }

        public void Client_OnReSubscriber(object sender, OnReSubscriberArgs e)
        {
            subAdder(e.ReSubscriber.SubscriptionPlan.ToString());

            Console.WriteLine("Resub " + e.ReSubscriber.SubscriptionPlan.ToString());
        }

        public void Client_OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            subAdder(e.GiftedSubscription.MsgParamSubPlan.ToString());

            Console.WriteLine("Gifted " + e.GiftedSubscription.MsgParamSubPlan.ToString());
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
                startDaysNum.Value = 0;
                startHoursNum.Value = 0;
                startMinutesNum.Value = 0;
                startSecondsNum.Value = 0;

                maxDaysNum.Value = 0;
                maxHoursNum.Value = 0;
                maxMinutesNum.Value = 0;
                maxSecondsNum.Value = 0;

                elapsedDays = 0;
                elapsedHours = 0;
                elapsedMinutes = 0;
                elapsedSeconds = 0;

                tier1NumCounter.Value = 0;
                tier2NumCounter.Value = 0;
                tier3NumCounter.Value = 0;

                estimatedEndLabel.Content = "Estimating...";
                timeLeftLabel.Content = "Estimating...";
                elapsedTimeLabel.Content = "Estimating...";

                startButton.IsEnabled = true;
                stopButton.IsEnabled = false;

                timer1.Close();

            
        }

        private void startButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (isStreamerSelected == true)
            {
                display.Show();
                timer1.Start();
                isStarted = true;
                days = startDays;
                hours = startHours;
                minutes = startMinutes;
                seconds = startSeconds;
                updateTime();
                startButton.IsEnabled = false;
                stopButton.IsEnabled = true;
                wakePychbot();
                pychConnected = isPychConnected.IsChecked.HasValue;
                SubTypeChoice = subType.SelectedIndex;
                tier1TypeChoice = tier1Type.SelectedIndex;
                tier2TypeChoice = tier2Type.SelectedIndex;
                tier3TypeChoice = tier3Type.SelectedIndex;


            }
            else
            {
                errorLabel.Content = "Please input your Stream Name to the Stream Selector";
            }
        }

        private void stopButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (isStarted == true)
            {
                timer1.Enabled = false;
                isStarted = false;
                updateTime();
                stopButton.Content = "Start";
            }
            else
            {
                timer1.Enabled = true;
                isStarted = true;
                updateTime();
                stopButton.Content = "Stop";

            }
        }

        private void primeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            primeTypeChoice = primeType.SelectedIndex;
        }

        private void tier3Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tier3TypeChoice = tier3Type.SelectedIndex;
        }

        private void tier2Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tier2TypeChoice = tier3Type.SelectedIndex;
        }

        private void tier1Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tier1TypeChoice = tier1Type.SelectedIndex;
        }

        private void comboBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            SubTypeChoice = subType.SelectedIndex;
            if(SubTypeChoice == 0)
            {
                timePerSubGroup.Visibility = Visibility.Visible;
            }
            else
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            subAdder(SubscriptionPlan.Tier1.ToString());
        }
    }
}

