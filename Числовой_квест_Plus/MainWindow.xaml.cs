using System;
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

namespace Числовой_квест_Plus
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // генератор случайных чисел
        private Random random = new Random();

        // переменные для здоровья игрока, количества зелий, золота и стрел
        private int playerHealth = 100;
        private int potions = 3;
        private int gold = 0;
        private int arrows = 5;

        // инвентарь
        private List<string> inventory = new List<string>();

        // карта подземелья
        private string[] dungeonMap = new string[10];

        // игрок начинает с нулевой комнаты
        private int currentRoom = 0;

        public MainWindow()
        {
            InitializeComponent();
            // инициализация подземелья
            InitializeDungeon();
            GameOutputTb.Text = "Добро пожаловать в Числовой квест PLUS! Начинаем приключение!";
            // описание текущей комнаты
            DescribeCurrentRoom();
        }

        // метод для инициализации подземелья
        private void InitializeDungeon()
        {
            string[] events = { "monster", "trap", "chest", "merchant", "empty" };
            for (int i = 0; i < dungeonMap.Length - 1; i++)
            {
                dungeonMap[i] = events[random.Next(events.Length)];
            }
            dungeonMap[9] = "boss"; // босс в последней комнате
        }

        // метод для описания текущей комнаты
        private void DescribeCurrentRoom()
        {
            GameOutputTb.Text += $"\nВы вошли в комнату {currentRoom + 1}.";

            // события
            switch (dungeonMap[currentRoom])
            {
                case "monster":
                    GameOutputTb.Text += "\nЗдесь есть монстр!";
                    break;
                case "trap":
                    GameOutputTb.Text += "\nВы видите ловушку!";
                    break;
                case "chest":
                    GameOutputTb.Text += "\nВы нашли сундук!";
                    break;
                case "merchant":
                    GameOutputTb.Text += "\nВы встретили торговца!";
                    break;
                case "empty":
                    GameOutputTb.Text += "\nПустая комната, ничего не произошло.";
                    break;
                case "boss":
                    GameOutputTb.Text += "\nВы встретили босса!";
                    break;
            }
            GameOutputTb.Text += "\nВведите команду (вперед, осмотреть, использовать зелье, инвентарь):";
        }

       
        // метод для перехода в некст комнату
        private void MoveForward()
        {
            if (currentRoom < dungeonMap.Length - 1)
            {
                currentRoom++; DescribeCurrentRoom();
                if (dungeonMap[currentRoom] == "monster")
                {
                    FightMonster();
                }
                else if (dungeonMap[currentRoom] == "trap")
                {
                    TriggerTrap();
                }
                else if (dungeonMap[currentRoom] == "chest")
                {
                    OpenChest();
                }
                else if (dungeonMap[currentRoom] == "merchant")
                {
                    VisitMerchant();
                }
                else if (dungeonMap[currentRoom] == "boss")
                {
                    FightBoss();
                }
            }
            else
            {
                GameOutputTb.Text += "\nВы достигли конца подземелья! Поздравляем!";
            }
        }

        // метод для боя с монстрами
        private void FightMonster()
        {
            int monsterHealth = random.Next(20, 51);
            GameOutputTb.Text += $"\nВы встретили монстра с {monsterHealth} HP!";
            while (monsterHealth > 0 && playerHealth > 0)
            {
                int damage = random.Next(10, 21); // Меч
                monsterHealth -= damage;
                GameOutputTb.Text += $"\nВы атаковали мечом и нанесли {damage} урона! Монстр HP: {monsterHealth}";

                if (monsterHealth > 0)
                {
                    int monsterDamage = random.Next(5, 16);
                    playerHealth -= monsterDamage;
                    GameOutputTb.Text += $"\nМонстр атаковал вас и нанес {monsterDamage} урона! Ваше здоровье: {playerHealth} HP";
                }
            }

            if (playerHealth > 0)
            {
                GameOutputTb.Text += "\nВы победили монстра!";
            }
            else
            {
                GameOutputTb.Text += "\nВы погибли! Игра окончена.";
            }
        }


        // метод для ловушек
        private void TriggerTrap()
        {
            int damage = random.Next(10, 21);
            playerHealth -= damage;
            GameOutputTb.Text += $"\nВы попали в ловушку и потеряли {damage} HP! Ваше здоровье: {playerHealth} HP";
            вс
            if (playerHealth <= 0)
            {
                GameOutputTb.Text += "\nВы погибли! Игра окончена.";
            }
        }


        // метод для сундуков и наград
        private void OpenChest()
        {
            int correctAnswer = random.Next(1, 11) + random.Next(1, 11); // Загадка для открытия сундука
            GameOutputTb.Text += $"\nЧтобы открыть сундук, решите загадку: сколько будет {correctAnswer - random.Next(1, 11)} + ?\nВведите ваш ответ в формате 'ответ: ваш_ответ'.";

            // Сохраним правильный ответ для последующей проверки
            UserInputTbx.Tag = correctAnswer; // Сохраняем правильный ответ в Tag элемента UserInput
        }

        // метод для проверки ответов
        private void CheckAnswer(string answer)
        {
            if (UserInputTbx.Tag is int correctAnswer && int.TryParse(answer, out int playerAnswer))
            {
                if (playerAnswer == correctAnswer)
                {
                    GameOutputTb.Text += "\nПравильный ответ! Вы открыли сундук!";
                    GiveReward(); // Даем награду за правильный ответ
                }
                else
                {
                    GameOutputTb.Text += "\nНеправильный ответ. Сундук остался закрытым.";
                }
            }
        }
         
        // поучение награды
        private void GiveReward()
        {
            string reward = GetRandomReward();
            GameOutputTb.Text += $"\nВы нашли {reward}!";

            if (reward == "potion")
            {
                if (inventory.Count < 5)
                {
                    inventory.Add("healing potion");
                    potions++;
                    GameOutputTb.Text += "\nВы получили зелье!";
                }
                else
                {
                    GameOutputTb.Text += "\nВаш инвентарь полон!";
                }
            }
            else if (reward == "gold")
            {
                gold += random.Next(10, 51);
                GameOutputTb.Text += $"\nВы получили золото! Теперь у вас {gold} золота.";
            }
            else if (reward == "arrows")
            {
                arrows += random.Next(1, 6);
                GameOutputTb.Text += $"\nВы получили стрелы! Теперь у вас {arrows} стрел.";
            }
        }
        

        // метод рандом награды
        private string GetRandomReward()
        {
            string[] rewards = { "potion", "gold", "arrows" };
            return rewards[random.Next(rewards.Length)];
        }

        private void VisitMerchant()
        {
            if (gold >= 30)
            {
                gold -= 30;
                potions++;
                inventory.Add("healing potion");
                GameOutputTb.Text += "\nВы купили зелье!";
            }
            else
            {
                GameOutputTb.Text += "\nУ вас недостаточно золота!";
            }
        }



        // метод для боя с боссом
        private void FightBoss()
        {
            int bossHealth = random.Next(50, 101);
            GameOutputTb.Text += $"\nВы встретили босса с {bossHealth} HP!";

            while (bossHealth > 0 && playerHealth > 0)
            {
                int damage = random.Next(10, 21); // Меч
                bossHealth -= damage;
                GameOutputTb.Text += $"\nВы атаковали босса и нанесли {damage} урона! Босс HP: {bossHealth}";

                if (bossHealth > 0)
                {
                    int bossDamage = random.Next(5, 16);
                    playerHealth -= bossDamage;
                    GameOutputTb.Text += $"\nБосс атаковал вас и нанес {bossDamage} урона! Ваше здоровье: {playerHealth} HP";
                }
            }

            if (playerHealth > 0)
            {
                GameOutputTb.Text += "\nВы победили босса! Поздравляем!";
            }
            else
            {
                GameOutputTb.Text += "\nВы погибли! Игра окончена.";
            }
        }


        // метод для использования зелья
        private void UsePotion()
        {
            if (potions > 0)
            {
                playerHealth += 20; // Восстанавливаем здоровье
                potions--;
                GameOutputTb.Text += $"\nВы использовали зелье. Ваше здоровье: {playerHealth} HP. Осталось зелий: {potions}.";
            }
            else
            {
                GameOutputTb.Text += "\nУ вас нет зелий для использования!";
            }
        }


        // метод для инвентаря
        private void ShowInventory()
        {
            string inventoryList = inventory.Count > 0 ? string.Join(", ", inventory) : "Пусто";
            GameOutputTb.Text += $"\nВаш инвентарь: {inventoryList}. Золото: {gold}, Стрелы: {arrows}. Здоровье: {playerHealth} HP.";
        }
    

        // метод для обрработчика события кнопки Отправить
        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            string command = UserInputTbx.Text.ToLower();
            UserInputTbx.Clear();

            switch (command)
            {
                case "вперед":
                    MoveForward();
                    break;
                case "осмотреть":
                    DescribeCurrentRoom();
                    break;
                case "использовать зелье":
                    UsePotion();
                    break;
                case "инвентарь":
                    ShowInventory();
                    break;
                default:
                    GameOutputTb.Text += "\nНеизвестная команда. Попробуйте снова.";
                    break;
            }
        }
    }
}
