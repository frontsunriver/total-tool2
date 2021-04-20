<?php

use Illuminate\Database\Seeder;
use App\Category;
class CategoryTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        Category::create([
        	'name'=> 'Uncategorizied',
        	'slug'=> 'uncategorizied',
        	'type'=> 0,
        	'user_id'=> 1,
        ]);


        Category::create([
            'name'=> 'Indian',
            'slug'=> 'indian',
            'avatar'=> asset('uploads/dummy/cuisine/indian.jpg'),
            'type'=> 2,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Chinese food',
            'slug'=> 'chinese-food',
            'avatar'=> asset('uploads/dummy/cuisine/chinese.jpg'),
            'type'=> 2,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Japanese',
            'slug'=> 'japanese-food',
            'avatar'=> asset('uploads/dummy/cuisine/japanese.jpg'),
            'type'=> 2,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Italian',
            'slug'=> 'italian',
            'avatar'=> asset('uploads/dummy/cuisine/italian.jpg'),
            'type'=> 2,
            'user_id'=> 1,
        ]); 

        Category::create([
            'name'=> 'mexican',
            'slug'=> 'mexican',
            'avatar'=> asset('uploads/dummy/cuisine/mexican.jpg'),
            'type'=> 2,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Appetizer',
            'slug'=> 'appetizer',
            'type'=> 1,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Salad',
            'slug'=> 'salad',
            'type'=> 1,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Soup',
            'slug'=> 'soup',
            'type'=> 1,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Rice Dish',
            'slug'=> 'rice',
            'type'=> 1,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Chicken Dish',
            'slug'=> 'rice',
            'type'=> 1,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Fish Dish',
            'slug'=> 'rice',
            'type'=> 1,
            'user_id'=> 1,
        ]);

        Category::create([
            'name'=> 'Mutton Dish',
            'slug'=> 'rice',
            'type'=> 1,
            'user_id'=> 1,
        ]);
    }
}
