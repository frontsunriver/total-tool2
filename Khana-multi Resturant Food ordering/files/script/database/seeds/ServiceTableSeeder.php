<?php

use Illuminate\Database\Seeder;
use App\Category;
use App\Terms;
use App\Post;
use App\Meta;
use App\Plan;

class ServiceTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
    	

    	$city=Terms::create([
    		'title'=>'Dhaka',
    		'slug'=>'dhaka',
    		'auth_id'=>1,
    		'type'=>2,
    		'status'=>1
    	]);
    	
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"23.8103","longitude":"90.4125","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/dhaka.jpg'),
        ]);


        $city=Terms::create([
            'title'=>'Chittagong',
            'slug'=>'chattogram',
            'auth_id'=>1,
            'type'=>2,
            'status'=>1
        ]);
        
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"22.3569","longitude":"91.7832","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/chittagong.jpg'),
        ]);

        // $city=Terms::create([
        //     'title'=>'Sylhet',
        //     'slug'=>'sylhet',
        //     'auth_id'=>1,
        //     'type'=>2,
        //     'status'=>1
        // ]);
        
        // Meta::create([
        //     'term_id'=>$city->id,
        //     'type'=>'excerpt',
        //     'content'=>'{"latitude":"24.8949","longitude":"91.8687","zoom":"12"}',
        // ]);
        // Meta::create([
        //     'term_id'=>$city->id,
        //     'type'=>'preview',
        //     'content'=>asset('uploads/dummy/city/sylhet.jpg'),
        // ]);

        $city=Terms::create([
            'title'=>'Rajshahi',
            'slug'=>'rajshahi',
            'auth_id'=>1,
            'type'=>2,
            'status'=>1
        ]);
        
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"24.3745","longitude":"88.6042","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/rajshahi.jpg'),
        ]);


        $city=Terms::create([
            'title'=>'Khulna',
            'slug'=>'khulna',
            'auth_id'=>1,
            'type'=>2,
            'status'=>1
        ]);
        
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"22.8456","longitude":"89.5403","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/khulna.jpg'),
        ]);


        $city=Terms::create([
            'title'=>'Rangpur',
            'slug'=>'rangpur',
            'auth_id'=>1,
            'type'=>2,
            'status'=>1
        ]);
        
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"25.7439","longitude":"89.2752","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/rangpur.jpg'),
        ]);


        $city=Terms::create([
            'title'=>'Feni',
            'slug'=>'feni',
            'auth_id'=>1,
            'type'=>2,
            'status'=>1
        ]);
        
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"23.0159","longitude":"91.3976","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/feni.jpg'),
        ]);


        $city=Terms::create([
            'title'=>'Bogra',
            'slug'=>'bogra',
            'auth_id'=>1,
            'type'=>2,
            'status'=>1
        ]);
        
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"24.8481","longitude":"89.3730","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/bogra.jpg'),
        ]);


        $city=Terms::create([
            'title'=>'Barisal',
            'slug'=>'barisal',
            'auth_id'=>1,
            'type'=>2,
            'status'=>1
        ]);
        
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'excerpt',
            'content'=>'{"latitude":"22.7010","longitude":"90.3535","zoom":"12"}',
        ]);
        Meta::create([
            'term_id'=>$city->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/city/barisal.jpg'),
        ]);





    	$lvl=Terms::create([
    		'title'=>'Seller Lavel 1',
    		'slug'=>'2300',
    		'auth_id'=>1,
    		'type'=>3,
    		'status'=>1,
            'count'=>1
    	]);
    	
        Meta::create([
            'term_id'=>$lvl->id,
            'type'=>'preview',
            'content'=>asset('uploads/dummy/label/label.png'),
        ]);

        
    	

    	
    }
}
