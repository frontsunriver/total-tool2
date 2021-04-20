<?php

use Illuminate\Database\Seeder;
use App\Customizer;
use Amcoders\Plugin\zoom\models\Meeting;

class CustomizerTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
       Customizer::create([
       		'key'=>'hero',
       		'theme_name'=> 'khana',
       		'value'=>'{"settings":{"hero_right_image":{"old_value":null,"new_value":"uploads\/2020-08-03-5f27e155e25e4.jpg"},"hero_right_title":{"old_value":"Hello Usa The Best 20% off","new_value":"Get 20% Off From Special Day"},"hero_title":{"old_value":null,"new_value":"Find Awesome Deals in Bangladesh"},"hero_des":{"old_value":null,"new_value":"Lists of top restaurants, cafes, pubs and bars in Melbourne, based on trends"},"button_title":{"old_value":null,"new_value":"Search"},"offer_title":{"old_value":null,"new_value":"Available Offer Right Now"},"hero_right_content":{"old_value":null,"new_value":"VALID ON SELECT ITEM"}}}',
       		'status' => 1
       ]);

       Customizer::create([
       		'key'=>'header',
       		'theme_name'=> 'khana',
       		'value'=>'{"settings":{"logo":{"old_value":null,"new_value":"uploads\/2020-08-03-5f27e25e2a680.png"},"header_pn":{"old_value":null,"new_value":"+825-285-9687"},"rider_team_title":{"old_value":null,"new_value":"Join Our Khana Rider Team!"},"rider_button_title":{"old_value":null,"new_value":"Apply Now"}}}',
       		'status' => 1
       ]);

       Customizer::create([
       		'key'=>'category',
       		'theme_name'=> 'khana',
       		'value'=>'{"settings":{"category_title":{"old_value":null,"new_value":"Browse By Category"}}}',
       		'status' => 1
       ]);

       Customizer::create([
       		'key'=>'best_restaurant',
       		'theme_name'=> 'khana',
       		'value'=>'{"settings":{"best_restaurant_title":{"old_value":null,"new_value":"Best Rated Restaurant"}}}',
       		'status' => 1
       ]);

       Customizer::create([
       		'key'=>'city_area',
       		'theme_name'=> 'khana',
       		'value'=>'{"settings":{"find_city_title":{"old_value":null,"new_value":"Find us in these cities and many more!"}}}',
       		'status' => 1
       ]);

       Customizer::create([
       		'key'=>'featured_resturent',
       		'theme_name'=> 'khana',
       		'value'=>'{"settings":{"featured_resturent_title":{"old_value":null,"new_value":"Featured Restaturents"}}}',
       		'status' => 1
       ]);

       Customizer::create([
       		'key'=>'footer',
       		'theme_name'=> 'khana',
       		'value'=>'{"settings":{"copyright_area":{"old_value":null,"new_value":"\u00a9 Copyright 2020 Amcoders. All rights reserved"}}}',
       		'status' => 1
       ]);
    }
}
