<?php

use Illuminate\Database\Seeder;
use App\Post;
use App\Meta;
use App\Terms;
use App\Category;
use App\User;
use Illuminate\Support\Facades\Hash;
use App\Usermeta;
use App\Plan;
use App\Featured;
use App\Location;
use App\Usercategory;
class BlogTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
       $terms=Terms::create([
          'title'=> 'terms and conditions',
          'slug'=> 'terms-and-conditions',
          'auth_id'=> 1,
          'type'=> 1,
          'status'=> 1,
       ]);

       Meta::create([
       	'term_id'=>$terms->id,
       	'type'=>'excerpt',
       	'content'=>'',
       ]);
       Meta::create([
        'term_id'=>$terms->id,
        'type'=>'content',
        'content'=>'',
       ]);


       $terms=Terms::create([
          'title'=> 'Privacy Policy',
          'slug'=> 'privacy-policy',
          'auth_id'=> 1,
          'type'=> 1,
          'status'=> 1,
       ]);

       Meta::create([
        'term_id'=>$terms->id,
        'type'=>'excerpt',
        'content'=>'',
       ]);
       Meta::create([
        'term_id'=>$terms->id,
        'type'=>'content',
        'content'=>'',
       ]);

       $terms=Terms::create([
          'title'=> 'Refund & Return Policy',
          'slug'=> 'refund-return-policy',
          'auth_id'=> 1,
          'type'=> 1,
          'status'=> 1,
       ]);

       Meta::create([
        'term_id'=>$terms->id,
        'type'=>'excerpt',
        'content'=>'',
       ]);
       Meta::create([
        'term_id'=>$terms->id,
        'type'=>'content',
        'content'=>'',
       ]);




       User::create([
          'role_id' => 2,
          'name' => 'Author',
          'slug' => 'author',
          'email' => 'author@author.com',
          'password' => Hash::make('rootadmin'),
        ]);


        $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Seasonal Tastes',
            'slug' => 'seasonal-tastes',
            'email' => 'seller1@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller1.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>2,
            'latitude'=>22.3244537,
            'longitude'=>91.81172319999999,
            'role_id'=>3,
        ]);



        // $seller = User::create([
        //     'role_id' => 3,
        //     'badge_id' => 9,
        //     'plan_id' => 1,
        //     'name' => 'Seasonal Tastes',
        //     'slug' => 'seasonal-tastes',
        //     'email' => 'seller2@email.com',
        //     'status' => 'approved',
        //     'email_verified_at' => '2020-08-02 22:11:23',
        //     'password' => Hash::make('rootadmin'),
        // ]);

        // UserMeta::create([
        //     'user_id'=>$seller->id,
        //     'type'=>'delivery',
        //     'content'=>'21',
        // ]);

        // UserMeta::create([
        //     'user_id'=>$seller->id,
        //     'type'=>'pickup',
        //     'content'=>'21',
        // ]);

        // UserMeta::create([
        //     'user_id'=>$seller->id,
        //     'type'=>'rattings',
        //     'content'=>'0',
        // ]);
        // UserMeta::create([
        //     'user_id'=>$seller->id,
        //     'type'=>'avg_rattings',
        //     'content'=>'0',
        // ]);
        // UserMeta::create([
        //     'user_id'=>$seller->id,
        //     'type'=>'info',
        //     'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        // ]);

        // UserMeta::create([
        //     'user_id'=>$seller->id,
        //     'type'=>'gallery',
        //     'content'=>null,
        // ]);

        // UserMeta::create([
        //     'user_id'=>$seller->id,
        //     'type'=>'preview',
        //     'content'=>asset('uploads/dummy/seller/seller1.jpg'),
        // ]);

        // Location::create([
        //     'user_id'=>$seller->id,
        //     'term_id'=>2,
        //     'latitude'=>22.3244537,
        //     'longitude'=>91.81172319999999,
        //     'role_id'=>3,
        // ]);


        $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Bar B Q Tonight',
            'slug' => 'bar-b-q-tonight',
            'email' => 'seller2@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller2.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>2,
            'latitude'=>22.332671097120553,
            'longitude'=>91.81163736931153,
            'role_id'=>3,
        ]);


        $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Lucknow Dhaka',
            'slug' => 'lucknow-dhaka',
            'email' => 'seller3@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller3.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>1,
            'latitude'=>23.804911781220447,
            'longitude'=>90.36312886452639,
            'role_id'=>3,
        ]);


        $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Shawarma Kitchen',
            'slug' => 'shawarma-kitchen',
            'email' => 'seller4@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller4.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>5,
            'latitude'=>25.742731921466344,
            'longitude'=>89.26655810046387,
            'role_id'=>3,
        ]);


        $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Taste n Best',
            'slug' => 'taste-n-best',
            'email' => 'seller5@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller5.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>6,
            'latitude'=>23.021679948799235,
            'longitude'=>91.38608178774415,
            'role_id'=>3,
        ]);


         $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Casuarina',
            'slug' => 'casuarina',
            'email' => 'seller6@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller6.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>7,
            'latitude'=>24.848973673437815,
            'longitude'=>89.36953007246095,
            'role_id'=>3,
        ]);
        

        $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Handi Korai',
            'slug' => 'handi-korai',
            'email' => 'seller7@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller7.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>8,
            'latitude'=>22.699378869630568,
            'longitude'=>90.35370859206546,
            'role_id'=>3,
        ]);


        $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Seyamoon Restaurant',
            'slug' => 'seyamoon-restaurant',
            'email' => 'seller8@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller8.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>3,
            'latitude'=>24.374983681345796,
            'longitude'=>88.59759995233154,
            'role_id'=>3,
        ]);


         $seller = User::create([
            'role_id' => 3,
            'badge_id' => 9,
            'plan_id' => 1,
            'name' => 'Saffron Restaurant',
            'slug' => 'saffron-restaurant',
            'email' => 'seller9@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'delivery',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'pickup',
            'content'=>'21',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'avg_rattings',
            'content'=>'0',
        ]);
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.","phone1":"12345678901","phone2":"12345678901","email1":"seller1@email.com","email2":"seller1@email.com","address_line":"Agrabad Commercial Area","full_address":"Agrabad Commercial Area, Chattogram, Bangladesh"}',
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'gallery',
            'content'=>null,
        ]);

        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/seller/seller9.jpg'),
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>4,
            'latitude'=>22.847499781575095,
            'longitude'=>89.53813921744383,
            'role_id'=>3,
        ]);




         $seller = User::create([
            'role_id' => 4,
            'name' => 'Rider',
            'slug' => 'rider',
            'email' => 'rider@email.com',
            'status' => 'approved',
            'email_verified_at' => '2020-08-02 22:11:23',
            'password' => Hash::make('rootadmin'),
        ]);

      
        
        UserMeta::create([
            'user_id'=>$seller->id,
            'type'=>'info',
            'content'=>'{"phone1":"+1 (157) 364-3678","phone2":"+1 (696) 983-9689","full_address":"Agrabad, Chattogram, Bangladesh"}',
        ]);

        Location::create([
            'user_id'=>$seller->id,
            'term_id'=>2,
            'latitude'=>22.3244537,
            'longitude'=>91.81172319999999,
            'role_id'=>4,
        ]);


        Featured::create([
          'user_id'=>11,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>10,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>9,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>8,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>7,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>6,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>5,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>4,
          'type'=>'featured_seller',
        ]);
        Featured::create([
          'user_id'=>3,
          'type'=>'featured_seller',
        ]);


        Usercategory::create([
          'user_id'=>4,
          'category_id'=>2,
        ]);

        Usercategory::create([
          'user_id'=>8,
          'category_id'=>3,
        ]);

        Usercategory::create([
          'user_id'=>9,
          'category_id'=>4,
        ]);
        Usercategory::create([
          'user_id'=>5,
          'category_id'=>5,
        ]);
        Usercategory::create([
          'user_id'=>11,
          'category_id'=>6,
        ]);
        Usercategory::create([
          'user_id'=>3,
          'category_id'=>7,
        ]);
        Usercategory::create([
          'user_id'=>10,
          'category_id'=>8,
        ]);
        Usercategory::create([
          'user_id'=>6,
          'category_id'=>12,
        ]);
        Usercategory::create([
          'user_id'=>7,
          'category_id'=>13,
        ]);



        Terms::create([
            'title'=>'bd24',
            'slug'=>'2020-12-12',
            'auth_id'=>10,
            'type'=>10,
            'count'=>10,
            'status'=> 1
        ]);
        Terms::create([
            'title'=>'hello21',
            'slug'=>'2020-12-12',
            'auth_id'=>3,
            'type'=>10,
            'count'=>10,
            'status'=> 1
        ]);
        Terms::create([
            'title'=>'brown21',
            'slug'=>'2020-12-12',
            'auth_id'=>4,
            'type'=>10,
            'count'=>10,
            'status'=> 1
        ]);
        Terms::create([
            'title'=>'testy24',
            'slug'=>'2020-12-12',
            'auth_id'=>5,
            'type'=>10,
            'count'=>10,
            'status'=> 1
        ]);
        Terms::create([
            'title'=>'hello',
            'slug'=>'2020-12-12',
            'auth_id'=>6,
            'type'=>10,
            'count'=>10,
            'status'=> 1
        ]);

    }
}
