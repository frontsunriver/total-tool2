<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Order extends Model
{
    protected $table="orders";

    public function orderlist()
    {
    	return $this->hasMany('App\Ordermeta')->with('products');
    }

    public function vendorinfo()
    {
    	return $this->hasOne('App\User','id','vendor_id')->with('location','info','avg_ratting');
    }

    public function riderinfo()
    {
       return $this->hasOne('App\User','id','rider_id')->with('location','phone','info');
    }

    public function coupon()
    {
        return $this->hasOne('App\Terms','id','coupon_id')->select('id','title');
    }

    public function orderlog()
    {
        return $this->hasMany('App\Orderlog');
    }

    public function resturentinfo()
    {
        return $this->hasOne('App\User','id','vendor_id')->select('id','name');
    }

    public function rideraccept()
    {
        return $this->hasOne('App\Riderlog')->where('status',1);
    }

    public function riderlog()
    {
        return $this->hasMany('App\Riderlog')->with('user');
    }


    public function review()
    {
        return $this->belongsTo('App\Comment','id','order_id');
    }

    public function vendor()
    {
        return $this->hasOne('App\User','id','vendor_id');
    }

    public function liveorder()
    {
        return $this->hasOne('App\Live');
    }
   
}
