<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Terms extends Model
{
	protected $table="terms";

	public function meta()
	{
		return 	$this->hasOne('App\Meta','term_id','id');
	}

	 public function restaurants(){
   		return $this->hasOne('App\User','id','auth_id')->where('role_id',3)->where('status','approved')->select('id','name')->with('avg_ratting','ratting','delivery','preview','coupons');
   	}

	
	public function categories()
    {
        return $this->belongsToMany('App\Category','post_category','term_id','category_id')->select('id');
    }
    public function postcategory()
    {
        return $this->hasMany('App\PostCategory','term_id');
    }

	public function comments()
	{
		return $this->hasMany('App\Comment','term_id','id');
	}

	public function user()
	{
		return $this->belongsTo('App\User','auth_id','id');
	}

	public function userwithpreview()
	{
		return $this->belongsTo('App\User','auth_id','id')->with('preview');
	}

	public function preview()
	{
	
	return $this->hasOne('App\Meta','term_id')->where('type','preview')->select('id','term_id','type','content');		
	}

	public function gallery()
	{

	return $this->belongsToMany('App\Media','post_relations','term_id','media_id')->where('post_relations.type','=','gallery')->select('id','url');
		
	}


	public function excerpt()
	{
		return $this->hasOne('App\Meta','term_id')->where('type','excerpt')->select('id','term_id','type','content');
	}

	public function content()
	{
		return $this->hasOne('App\Meta','term_id')->where('type','content')->select('id','term_id','type','content');
	}

	public function price()
	{
		return $this->hasOne('App\Productmeta','term_id')->select('id','term_id','price');
	}

	public function addonid(){
		return $this->hasMany('App\Addon','term_id')->select('term_id','addon_id');
	}

	public function addons(){
	  return $this->belongsToMany('App\Terms','addons','term_id','addon_id')->with('price')->select('id','title');
	}

	public function Location()
	{
		return $this->hasMany('App\Location','term_id')->where('role_id',3)->with('restaurants');
	}


	public function coupon()
	{
		return $this->hasMany('App\Order','coupon_id');
		
	}

	public function badgeusers()
	{
		return $this->hasOne('App\User','badge_id','id');
	}

	public function userslocation()
	{
		return $this->hasMany('App\Location','term_id');
	}

	public function order()
	{
		return $this->hasMany('App\Ordermeta','term_id','id');
	}

	public function Locationcount()
	{
		return $this->hasMany('App\Location','term_id')->where('role_id',3);
	}


}
