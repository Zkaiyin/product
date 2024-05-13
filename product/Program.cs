﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace product
{


    class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = ReadProductsFromCsv("product.csv");

            if (products.Any())
            {
                Console.WriteLine("商品列表：");
                DisplayProducts(products);

                Console.WriteLine();
                Console.WriteLine("所有商品的總價格");
                decimal totalPrice = products.Sum(p => p.Amount * p.UnitPrice);
                Console.WriteLine($"總價格：{totalPrice}");

                Console.WriteLine();
                Console.WriteLine("所有商品的平均價格");
                decimal averagePrice = products.Average(p => p.UnitPrice);
                Console.WriteLine($"平均價格：{averagePrice}");

                Console.WriteLine();
                Console.WriteLine("商品的總數量");
                int totalAmount = products.Sum(p => p.Amount);
                Console.WriteLine($"總數量：{totalAmount}");

                Console.WriteLine();
                Console.WriteLine("商品的平均數量");
                double averageAmount = products.Average(p => p.Amount);
                Console.WriteLine($"平均數量：{averageAmount}");

                Console.WriteLine();
                Console.WriteLine("最貴產品");
                var mostExpensiveProduct = products.OrderByDescending(p => p.UnitPrice).FirstOrDefault();
                Console.WriteLine($"最貴的：{mostExpensiveProduct.ProductName}, 價格：{mostExpensiveProduct.UnitPrice}");

                Console.WriteLine();
                Console.WriteLine("最便宜產品");
                var cheapestProduct = products.OrderBy(p => p.UnitPrice).FirstOrDefault();
                Console.WriteLine($"最便宜的：{cheapestProduct.ProductName}, 價格：{cheapestProduct.UnitPrice}");

                Console.WriteLine();
                Console.WriteLine("3C 的總價格");
                decimal total3CPrice = products.Where(p => p.ProductType == "3C").Sum(p => p.Amount * p.UnitPrice);
                Console.WriteLine($"3C 的總價格：{total3CPrice}");

                Console.WriteLine();
                Console.WriteLine("飲料和食品的總價");
                decimal totalPriceFoodAndDrink = products
                    .Where(p => p.ProductType == "飲料" || p.ProductType == "食品")
                    .Sum(p => p.Amount * p.UnitPrice);
                Console.WriteLine($"飲料和食品的總價: {totalPriceFoodAndDrink}");

                Console.WriteLine();
                Console.WriteLine("各類別底下價格大於 1000 的產品");
                var expensiveProductsByCategory = products
                    .GroupBy(p => p.ProductType)
                    .ToDictionary(g => g.Key, g => g.Where(p => p.UnitPrice > 1000).ToList());

                foreach (var category in expensiveProductsByCategory)
                {
                    if (category.Value.Any())
                    {
                        Console.WriteLine($"   {category.Key} 類別底下有價格大於 1000 的商品是:");
                        foreach (var product in category.Value)
                        {
                            Console.WriteLine($"      - {product.ProductName}, 價格: {product.UnitPrice}");
                        }
                        // 計算平均價格
                        decimal averagePriceByCategory = category.Value.Average(p => p.UnitPrice);
                        Console.WriteLine($"   平均價格: {averagePriceByCategory}");
                    }
                    else
                    {
                        Console.WriteLine($"   {category.Key} 類別底下沒有價格大於 1000 的商品");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("產品單價由高到低排列");
                var productsSortedByPriceDescending = products.OrderByDescending(p => p.UnitPrice);
                foreach (var product in productsSortedByPriceDescending)
                {
                    Console.WriteLine($"   - {product.ProductName}, 價格: {product.UnitPrice}");
                }

                Console.WriteLine();
                Console.WriteLine("產品數量由低到高排列");
                var productsSortedByAmountAscending = products.OrderBy(p => p.Amount);
                foreach (var product in productsSortedByAmountAscending)
                {
                    Console.WriteLine($"   - {product.ProductName}, 數量: {product.Amount}");
                }
            }
            else
            {
                Console.WriteLine("沒有商品資料。");
            }


            static List<Product> ReadProductsFromCsv(string filePath)
            {
                List<Product> products = new List<Product>();

                try
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {

                        sr.ReadLine();
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            Product product = new Product
                            {
                                ProductNumber = parts[0],
                                ProductName = parts[1],
                                Amount = int.Parse(parts[2]),
                                UnitPrice = decimal.Parse(parts[3]),
                                ProductType = parts[4]
                            };
                            products.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"發生錯誤：{ex.Message}");
                }

                return products;
            }

            static void DisplayProducts(List<Product> products)
            {
                int pageSize = 4;
                int totalPages = (int)Math.Ceiling((double)products.Count / pageSize);

                for (int page = 1; page <= totalPages; page++)
                {
                    Console.WriteLine($"第 {page} 頁：");
                    var pageProducts = products.Skip((page - 1) * pageSize).Take(pageSize);
                    foreach (var product in pageProducts)
                    {
                        Console.WriteLine($"產品編號：{product.ProductNumber}, 產品：{product.ProductName}, 數量：{product.Amount}, 單價：{product.UnitPrice}, 類別：{product.ProductType}");
                    }
                }
            }
        }
    }
}

    


