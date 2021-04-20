<?php

use Illuminate\Database\Seeder;
use App\User;
use Illuminate\Support\Facades\Hash;
use App\UserMeta;
use App\Plan;
use App\Location;
use Spatie\Permission\Models\Role;
use Spatie\Permission\Models\Permission;
class UsersTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        Plan::create([
            'name'=>'Free Membership',
            'duration'=>'month',
            's_price'=>'0',
            'img_limit'=>100,
            'commission'=>37.5
        ]);
        Plan::create([
            'name'=>'Silver Membership',
            'duration'=>'month',
            's_price'=>'0',
            'img_limit'=>250,
            'commission'=>25,
            'f_resturent'=>0,
            'table_book'=>1
        ]);
        Plan::create([
            'name'=>'Gold Membership',
            'duration'=>'month',
            's_price'=>'0',
            'img_limit'=>350,
            'commission'=>20,
            'f_resturent'=>1,
            'table_book'=>0
        ]);
        Plan::create([
            'name'=>'Platinum Membership',
            'duration'=>'month',
            's_price'=>'0',
            'img_limit'=>500,
            'commission'=>12.5,
            'f_resturent'=>1,
            'table_book'=>1
        ]);


        $super= User::create([
        	'role_id' => 1,
        	'name' => 'Admin',
        	'slug' => 'admin',
        	'email' => 'admin@admin.com',
        	'password' => Hash::make('rootadmin'),
        ]);


        $roleSuperAdmin = Role::create(['name' => 'superadmin']);
        //create permission
        $permissions = [
            [
                'group_name' => 'dashboard',
                'permissions' => [
                    'dashboard'
                ]
            ],
             [
                'group_name' => 'update',
                'permissions' => [
                    'update'
                ]
            ],
            [
                'group_name' => 'admin',
                'permissions' => [
                    'admin.create',
                    'admin.edit',
                    'admin.update',
                    'admin.delete',
                    'admin.list',

                ]
            ],
            [
                'group_name' => 'role',
                'permissions' => [
                    'role.create',
                    'role.edit',
                    'role.update',
                    'role.delete',
                    'role.list',

                ]
            ],

            [
                'group_name' => 'Media',
                'permissions' => [
                    'media.list',
                    'media.upload',
                    'media.delete',
                ]
            ],
            [
                'group_name' => 'Page',
                'permissions' => [
                    'page.list',
                    'page.create',
                    'page.edit',
                    'page.delete',
                    
                ]
            ],
            [
                'group_name' => 'Products',
                'permissions' => [
                    'product.list',
                    'product.delete',
                    'product.category',
                    
                ]
            ],
            [
                'group_name' => 'Restaurant',
                'permissions' => [
                    'resturents.requests',
                    'resturents.view',
                    'all.resturents',
                    'manage.review',
                ]
            ],
            [
                'group_name' => 'Rider',
                'permissions' => [
                    'rider.request',
                    'all.rider',
                ]
            ], 
            [
                'group_name' => 'Customer',
                'permissions' => [
                    'customer.list',
                    'customer.edit',                  
                ]
            ],
            [
                'group_name' => 'Payout',
                'permissions' => [
                    'payout.request',
                    'payout.history',
                    'payout.account',
                    'payout.view',

                ]
            ],
            [
                'group_name' => 'Orders',
                'permissions' => [
                    'order.list',                                     
                    'order.control',                                     
                ]
            ],
            [
                'group_name' => 'Plan',
                'permissions' => [
                    'plan.create',                                     
                    'plan.list',                                     
                    'plan.view',                                     
                    'plan.edit',                                     
                    'plan.delete',                                     
                    'payment.request',                                     
                    'payment.make',                                     

                ]
            ],
            [
                'group_name' => 'Location',
                'permissions' => [
                    'location.create',                                     
                    'location.list',                                     
                    'location.edit',
                    'location.delete'                                     

                ]
            ],
            [
                'group_name' => 'Badge',
                'permissions' => [
                    'badge.control',                                     

                ]
            ],
            [
                'group_name' => 'Featured',
                'permissions' => [
                    'featured.control',                                     

                ]
            ],
            [
                'group_name' => 'Reports',
                'permissions' => [
                    'earning.order.report',                                     
                    'earning.delivery.report',                                     
                    'earning.subscription.report',                                     

                ]
            ],                
             [
                'group_name' => 'Appearance',
                'permissions' => [
                    'theme',                                     
                    'menu',                                     
                ]
            ],
            [
                'group_name' => 'Plugin',
                'permissions' => [
                    'plugin.control',                                     
                                                       
                ]
            ],
            [
                'group_name' => 'Settings',
                'permissions' => [
                    'site.settings',                                     
                    'seo',                                     
                    'file.system',                                     
                    'system.settings',                                     
                    'payment.settings',                                     
                                                         
                ]
            ],                 
            
                       
            [
                'group_name' => 'language',
                'permissions' => [
                    'language.control',
                ]
            ]

        ];

        //assign permission

        foreach ($permissions as $key => $row) {


            foreach ($row['permissions'] as $per) {
                $permission = Permission::create(['name' => $per, 'group_name' => $row['group_name']]);
                $roleSuperAdmin->givePermissionTo($permission);
                $permission->assignRole($roleSuperAdmin);
                $super->assignRole($roleSuperAdmin);
            }
        }


        
    }
}
