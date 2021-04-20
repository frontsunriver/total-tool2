<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use App\Models\User;

class UserSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {   
         // create super-admin users
        $user = User::firstOrCreate([
            'id' => '1'],
            ['username' => 'admin',
            'name' => 'Admin Name',
            'email' => 'your@email.com',
            'password' => bcrypt('password')
        ]);

        User::where('id', 1)->update(['role' => 'super-admin']);
    }
}
