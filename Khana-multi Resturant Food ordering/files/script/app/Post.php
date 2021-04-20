<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Post extends Model
{
	protected $table="posts";

	public function term()
	{
		return $this->hasMany('App\Terms');
	}

}
