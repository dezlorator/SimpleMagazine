﻿using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PetStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();
           // context.Database.Migrate();

            if (!context.Products.Any() && !context.StockItems.Any())
            {
                #region Pharmacy

                //Product p1 = new Product
                //{
                //    Name = "Панкреазим",
                //    Description = "Полиферментный препарат. Панкреатические ферменты (липаза, амилаза и протеаза), входящие в его состав, способствуют расщеплению жиров, белков и углеводов, что способствует их полному всасыванию в тонком кишечнике. При заболеваниях поджелудочной железы препарат компенсирует недостаточность ее внешнесекреторной функции и способствует улучшению процесса пищеварения. Таблетки покрыты защитной оболочкой, нерастворимой в кислой среде желудка, защищающей пищеварительные ферменты от разрушения в желудке.",
                //    Category = "Ферменты",
                //    Price = 100,
                //    ImageId = ""
                //};

                //Product p2 = new Product
                //{
                //    Name = "Но-шпа",
                //    Description = "Дротаверин — производное изохинолина, который оказывает спазмолитическое действие на гладкие мышцы путем угнетения действия фермента ФДЭ ІV, вызывая увеличение концентрации цАМФ и благодаря инактивации легкой цепочки киназы миозина (MLCK) приводит к расслаблению гладких мышц.",
                //    Category = "Лекарства от спазмов",
                //    Price = 55,
                //    ImageId = ""
                //};

                //Product p3 = new Product
                //{
                //    Name = "Мезим",
                //    Description = "Действующим веществом лекарственного средства Мезим форте является порошок из поджелудочных желез (панкреатин) млекопитающих, обычно свиней, который, кроме экскреторных панкреатических ферментов (липазы, альфа-амилазы, трипсина и химотрипсина), содержит и другие ферменты. Панкреатин также содержит другие сопутствующие вещества, не имеющие ферментативной активности.",
                //    Category = "Ферменты",
                //    Price = 45,
                //    ImageId = ""
                //};

                //Product p4 = new Product
                //{
                //    Name = "Спазмалгон",
                //    Description = "Спазмалгон — комбинированный препарат с анальгезирующим, спазмолитическим (папавериноподобным), холинолитическим (атропиноподобным) и некоторым противовоспалительным действием.",
                //    Category = "Лекарства от спазмов",
                //    Price = 25,
                //    ImageId = ""
                //};

                //Product p5 = new Product
                //{
                //    Name = "Валериана",
                //    Description = "Препарат снижает возбудимость ЦНС. Действие обусловлено содержанием эфирного масла, большая часть которого — сложный эфир спирта борнеола и изовалериановой кислоты. Седативные свойства имеют также валепотриаты и алкалоиды — валерин и хотинин. Седативное действие проявляется медленно, но достаточно стабильно. Валериановая кислота и валепотриаты оказывают слабое спазмолитическое действие.",
                //    Category = "Успокоительные",
                //    Price = 72,
                //    ImageId = ""
                //};

                //Product p6 = new Product
                //{
                //    Name = "Анальгин",
                //    Description = "Анальгин (метамизола натриевая соль) проявляет аналгетическое, жаропонижающее и противовоспалительное действие. Аналгетический эффект обусловлен ингибицией циклооксигеназы и блокированием синтеза простагландинов из арахидоновой кислоты, которые принимают участие в формировании реакции на болевые раздражители (брадикинины, простагландины); замедлением проведения экстра- и проприоцептивных болевых импульсов в центральной нервной системе, повышением порога возбудимости таламических центров болевой чувствительности и ослаблением реакции структур головного мозга, отвечающих за восприятие боли.",
                //    Category = "Обезбаливающие",
                //    Price = 72,
                //    ImageId = ""
                //};

                //Product p7 = new Product
                //{
                //    Name = "Нурофен",
                //    Description = "Ибупрофен — это НПВП, производное пропионовой кислоты, который продемонстрировал свою эффективность путем угнетения синтеза простагландинов. У человека ибупрофен уменьшает выраженность боли при воспалении, отеки и лихорадку. Кроме того, ибупрофен обратимо угнетает агрегацию тромбоцитов.",
                //    Category = "Обезбаливающие",
                //    Price = 55,
                //    ImageId = ""
                //};

                //Product p8 = new Product
                //{
                //    Name = "Стрепсилс",
                //    Description = "Препарат обладает антисептическими свойствами. Активен в отношении широкого спектра грамположительных и грамотрицательных микроорганизмов; оказывает противогрибковое действие. Эффективность препарата обусловлена наличием двух антибактериальных компонентов широкого спектра действия, которые облегчают боль в горле и уменьшают проявления воспаления. Амилметакрезол разрушает структуру белков бактерий, что обеспечивает бактерицидное действие. 2,4-дихлорбензиловый спирт проявляет бактериостатический эффект за счет обезвоживания бактериальной клетки.",
                //    Category = "Обезбаливающие",
                //    Price = 70,
                //    ImageId = ""
                //};

                //Product p9 = new Product
                //{
                //    Name = "Цитрамон",
                //    Description = "Цитрамон — комбинированный препарат, который оказывает анальгезирующее, жаропонижающее и противовоспалительное действие. Компоненты, входящие в его состав, усиливают эффекты друг друга.",
                //    Category = "Обезбаливающие",
                //    Price = 45,
                //    ImageId = ""
                //};

                #endregion

                #region PetStore

                var category1 = new CategoryNode
                {
                    Name = "Все товары",
                    IsRoot = true,
                    Children = new List<CategoryNode>()
                };

                var category2 = new CategoryNode
                {
                    Name = "Мебель",
                    Children = new List<CategoryNode>()
                };

                category1.Children.Add(category2);

                var category3 = new CategoryNode
                {
                    Name = "Оружие",
                    Children = new List<CategoryNode>()
                };

                category1.Children.Add(category3);

                var category4 = new CategoryNode
                {
                    Name = "Холодное оружие",
                    Children = new List<CategoryNode>()
                };

                category3.Children.Add(category4);

                var category5 = new CategoryNode
                {
                    Name = "Огнетрельное оружие",
                    Children = new List<CategoryNode>()
                };

                category3.Children.Add(category5);

                var category6 = new CategoryNode
                {
                    Name = "Посуда",
                    Children = new List<CategoryNode>()
                };

                category1.Children.Add(category6);

                var category7 = new CategoryNode
                {
                    Name = "Ложки",
                    Children = new List<CategoryNode>()
                };

                category6.Children.Add(category7);

                var category8 = new CategoryNode
                {
                    Name = "Тарелки",
                    Children = new List<CategoryNode>()
                };

                category6.Children.Add(category8);

                context.Categories.Add(category1);

                ProductExtended p1 = new ProductExtended
                {
                    Product = new Product
                    {
                        Name = "Древний ятаган печенегов",
                        Description = "Принадлежал знаменитому хану Котяну, который сражался в битве на реке Калке бок о бок с князьями Руси",
                        Category = category4,
                        Price = 2500,
                        ImageId = ""
                    },
                    LongDescription = "Принадлежал знаменитому хану Котяну, который сражался в битве на реке Калке бок о бок с князьями Руси",
                    Manufacturer = "Древние мастера",
                    OriginCountry = "Половецкая орда"
                };

                ProductExtended p2 = new ProductExtended
                {
                    Product = new Product
                    {
                        Name = "Пернач",
                        Description = "Старинный пернач принадлежал одному из предводителей национально вызволительной войны. Был найден про Каменцом-Полольским",
                        Category = category4,
                        Price = 469,
                        ImageId = ""
                    },
                    LongDescription = "Старинный пернач принадлежал одному из предводителей национально вызволительной войны. Был найден про Каменцом-Полольским",
                    Manufacturer = "Украинские мастера",
                    OriginCountry = "Украина"
                };

                ProductExtended p3 = new ProductExtended
                {
                    Product = new Product
                    {
                        Name = "Сервиз зажиточного горожанина",
                        Description = "Сервиз принадлежал зажиточному горожанину Киева времен Екатерины Великой, и был гордостью дома.",
                        Category = category7,
                        Price = 78,
                        ImageId = ""
                    },
                    LongDescription = "Сервиз принадлежал зажиточному горожанину Киева времен Екатерины Великой, и был гордостью дома.",
                    Manufacturer = "Русские мастера",
                    OriginCountry = "Росийская Империя"
                };

                ProductExtended p4 = new ProductExtended
                {
                    Product = new Product
                    {
                        Name = "Шкаф 4-х дверный Florenza с зеркалами 03-49065",
                        Description = "Шкаф Florenza от румынского бренда Mobex изготовлен из массива клёна в цвете орех, прошпонирован и вскрыт высококачественным лаком. Обладает двумя выдвижными ящичками, двумя деревянными и двумя зеркальными дверцами. Декорирован золотой патиной, кариатидами, резными орнаментами, узорами и погонажем ручной шлифовки, которые сделаны в стиле Ренессанс - эпохи Возрождения ХIХ века. Воистину королевский гарнитур является прекрасным дополнением к спальне этой же серии.",
                        Category = category2,
                        Price = 83,
                        ImageId = ""
                    },
                    LongDescription = "Шкаф Florenza от румынского бренда Mobex изготовлен из массива клёна в цвете орех, прошпонирован и вскрыт высококачественным лаком. Обладает двумя выдвижными ящичками, двумя деревянными и двумя зеркальными дверцами. Декорирован золотой патиной, кариатидами, резными орнаментами, узорами и погонажем ручной шлифовки, которые сделаны в стиле Ренессанс - эпохи Возрождения ХIХ века. Воистину королевский гарнитур является прекрасным дополнением к спальне этой же серии.",
                    Manufacturer = "Румынский мастер",
                    OriginCountry = "Румыния"
                };

                ProductExtended p5 = new ProductExtended
                {
                    Product = new Product
                    {
                        Name = "Стул Florenza с подлокотниками 03-49042",
                        Description = "Стул Florenza от румынского бренда Mobex изготовлен из массива клёна в цвете орех, прошпонирован и вскрыт высококачественным лаком. Обладает ровными прочными ножками, удобными подлокотниками и комфортным посадочным местом, которое обито натуральной кожей и задекорировано металлическими заклепками. Декорирован небольшой короной, золотой патиной, резными орнаментами, узорами и погонажем ручной шлифовки, которые сделаны в стиле Ренессанс - эпохи Возрождения ХIХ века. Воистину королевский гарнитур является прекрасным дополнением к рабочему кабинету этой же серии.",
                        Category = category2,
                        Price = 3260,
                        ImageId = ""
                    },
                    LongDescription = "Стул Florenza от румынского бренда Mobex изготовлен из массива клёна в цвете орех, прошпонирован и вскрыт высококачественным лаком. Обладает ровными прочными ножками, удобными подлокотниками и комфортным посадочным местом, которое обито натуральной кожей и задекорировано металлическими заклепками. Декорирован небольшой короной, золотой патиной, резными орнаментами, узорами и погонажем ручной шлифовки, которые сделаны в стиле Ренессанс - эпохи Возрождения ХIХ века. Воистину королевский гарнитур является прекрасным дополнением к рабочему кабинету этой же серии.",
                    Manufacturer = "Румынский мастер",
                    OriginCountry = "Румыния"
                };

                ProductExtended p6 = new ProductExtended
                {
                    Product = new Product
                    {
                        Name = "Стол письменный Florenza 2 тумбы 03-49041",
                        Description = "Стол письменный Florenza от румынского бренда Mobex изготовлен из массива клёна в цвете орех, прошпонирован и вскрыт высококачественным лаком. Обладает прямоугольной столешницей, двумя тумбами и выдвижным ящиком. Декорирован золотой патиной, кариатидами, резными орнаментами, узорами и погонажем ручной шлифовки, которые сделаны в стиле Ренессанс - эпохи Возрождения ХIХ века. Воистину королевский гарнитур является прекрасным дополнением к рабочему кабинету этой же серии.",
                        Category = category4,
                        Price = 31,
                        ImageId = ""
                    },
                    LongDescription = "Стол письменный Florenza от румынского бренда Mobex изготовлен из массива клёна в цвете орех, прошпонирован и вскрыт высококачественным лаком. Обладает прямоугольной столешницей, двумя тумбами и выдвижным ящиком. Декорирован золотой патиной, кариатидами, резными орнаментами, узорами и погонажем ручной шлифовки, которые сделаны в стиле Ренессанс - эпохи Возрождения ХIХ века. Воистину королевский гарнитур является прекрасным дополнением к рабочему кабинету этой же серии.",
                    Manufacturer = "Румынский мастер",
                    OriginCountry = "Румыния"
                };

                ProductExtended p7 = new ProductExtended
                {
                    Product = new Product
                    {
                        Name = "Кумган. Антикварный старинный медный кувшин ",
                        Description = "Антикварный Исламский кувшин 18-19 век",
                        Category = category8,
                        Price = 2184,
                        ImageId = ""
                    },
                    LongDescription = "Антикварный Исламский кувшин 18-19 век",
                    Manufacturer = "1st Choice",
                    OriginCountry = "Китай"
                };

                #endregion

                context.ProductsExtended.AddRange(p1, p2, p3, p4, p5, p6, p7);

                context.StockItems.AddRange(
                    new Stock { Product = p1.Product, Quantity = 10 },
                    new Stock { Product = p2.Product, Quantity = 5 },
                    new Stock { Product = p3.Product, Quantity = 12 },
                    new Stock { Product = p4.Product, Quantity = 3 },
                    new Stock { Product = p5.Product, Quantity = 6 },
                    new Stock { Product = p6.Product, Quantity = 7 },
                    new Stock { Product = p7.Product, Quantity = 4 }
                    );
                context.SaveChanges();
            }
        }
    }
}