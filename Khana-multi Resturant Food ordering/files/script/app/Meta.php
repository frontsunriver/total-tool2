<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Meta extends Model
{
    protected $table="meta";

    public function term()
    {
    	return $this->hasMany('App\Terms');
    }
}
