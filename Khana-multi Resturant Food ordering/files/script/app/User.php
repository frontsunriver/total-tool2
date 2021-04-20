<?php

namespace App;

use Illuminate\Contracts\Auth\MustVerifyEmail;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Notifications\Notifiable;
use Laravel\Sanctum\HasApiTokens;
use Illuminate\Support\Facades\DB;
use Spatie\Permission\Traits\HasRoles;
class User extends Authenticatable implements MustVerifyEmail
{
    use HasApiTokens, Notifiable,HasRoles;

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = [
        'name', 'email', 'password','role_id'
    ];

    /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'password', 'remember_token',
    ];

    /**
     * The attributes that should be cast to native types.
     *
     * @var array
     */
    protected $casts = [
        'email_verified_at' => 'datetime',
    ];


    public static function getpermissionGroups()
    {
        $permission_groups = DB::table('permissions')
        ->select('group_name as name')
        ->groupBy('group_name')
        ->get();
        return $permission_groups;
    }

    public static function getPermissionGroup()
    {
        return $permission_groups = DB::table('permissions')->select('group_name')->groupBy('group_name')->get();
    }
    public static function getpermissionsByGroupName($group_name)
    {
        $permissions = DB::table('permissions')
        ->select('name', 'id')
        ->where('group_name', $group_name)
        ->get();
        return $permissions;
    }


    public static function roleHasPermissions($role, $permissions)
    {
        $hasPermission = true;
        foreach ($permissions as $permission) {
            if (!$role->hasPermissionTo($permission->name)) {
                $hasPermission = false;
                return $hasPermission;
            }
        }
        return $hasPermission;
    }



    public function term()
    {
        return $this->hasMany('App\Terms','auth_id','id');
    }

    public function role()
    {
        return $this->belongsTo('App\Role');
    }

    public function usermeta(){

        return $this->hasOne('App\Usermeta','user_id','id')->select('type','content');
    }

    public function phone()
    {
        return $this->hasOne('App\Usermeta','user_id','id')->where('type','phone')->select('user_id','content');
    }

    public function plan()
    {
     return $this->belongsTo('App\Plan','plan_id');
 }

 public function avg_ratting()
 {
    return $this->hasOne('App\Usermeta','user_id')->where('type','avg_rattings')->select('user_id','content');
    
}
public function ratting()
{
    return $this->hasOne('App\Usermeta','user_id')->where('type','rattings')->select('user_id','content');
    
}

public function products()
{
    return $this->hasMany('App\Terms','auth_id')->where('type',6)->with('preview');   
}

public function featured_seller()
{
    return $this->hasMany('App\Featured','user_id ')->where('type','featured_seller');
    
}

public function info()
{
    return $this->hasOne('App\Usermeta','user_id')->where('type','info')->select('user_id','content');  
}

public function preview()
{
    return $this->hasOne('App\Usermeta','user_id')->where('type','preview')->select('user_id','content');  
}

public function allinfo()
{
    return $this->hasMany('App\Usermeta','user_id');  
}

public function livechat()
{
    return $this->hasOne('App\Usermeta','user_id')->where('type','livechat')->select('user_id','content');  
}

public function delivery()
{
    return $this->hasOne('App\Usermeta','user_id')->where('type','delivery')->select('user_id','content');  
}

public function pickup()
{
    return $this->hasOne('App\Usermeta','user_id')->where('type','pickup')->select('user_id','content');  
}

public function gallery()
{
    return $this->hasOne('App\Usermeta','user_id')->where('type','gallery')->select('user_id','content');  
}

public function location()
{
    return $this->hasOne('App\Location','user_id')->select('user_id','term_id','latitude','longitude');  
}

public function city()
{
    return $this->hasOne('App\Location','user_id')->with('city');  
}


public function usercategory()
{
    return $this->hasMany('App\Usercategory','user_id')->select('user_id','category_id');  
}


public function shopcategory()
{
 return $this->belongsToMany('App\Category','user_category','user_id','category_id')->select('name','slug');
}



public function shopday()
{
    return $this->hasMany('App\Shopday','user_id')->select('user_id','status','day','opening','close');
}


public function resturentlocation()
{
   return $this->hasOne('App\Location','user_id')->select('user_id','term_id','latitude','longitude');
}

public function resturentlocationwithcity()
{
   return $this->hasOne('App\Location','user_id')->with('area')->select('user_id','term_id','latitude','longitude');
}

public function usersaas()
{
    return $this->hasOne('App\Plan','id','plan_id')->select('id','name','f_resturent','table_book','img_limit','commission');
}

public function Onesignal()
{
    return $this->hasMany('App\Onesignal');
}

public function badge()
{
    return $this->hasOne('App\Terms','id','badge_id')->select('id','title')->with('preview');
}

public function user_reviews()
{
    return $this->hasMany('App\Comment');
}

public function vendor_reviews()
{
    return $this->hasMany('App\Comment','vendor_id','id')->with('comment_meta');
}

public function five_ratting()
{
    return $this->hasMany('App\Commentmeta','vendor_id','id')->where('star_rate',5);
}

public function four_ratting()
{
    return $this->hasMany('App\Commentmeta','vendor_id','id')->where('star_rate',4);
}

public function three_ratting()
{
    return $this->hasMany('App\Commentmeta','vendor_id','id')->where('star_rate',3);
}

public function two_ratting()
{
    return $this->hasMany('App\Commentmeta','vendor_id','id')->where('star_rate',2);
}

public function one_ratting()
{
    return $this->hasMany('App\Commentmeta','vendor_id','id')->where('star_rate',1);
}


public function saasmeta()
{
    return $this->hasMany('App\Userplan');
}

public function coupons()
{
    return $this->hasOne('App\Terms','auth_id')->where('type',10)->where('status',1)->select('auth_id','title','count');
}

public function shoptag()
{
 return $this->belongsToMany('App\Category','user_category','user_id','category_id')->where('type',2)->select('name','slug','type');
}

}
