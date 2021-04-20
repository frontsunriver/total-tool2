<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\Auth;
use Carbon\Carbon;
use Illuminate\Support\Str;

class Post extends Model
{
    use HasFactory;
    
    protected $guarded = ['id'];

    //Like system
    public function likes()
    {
        return $this->morphToMany('App\Models\User', 'likeable')->whereDeletedAt(null);
    }

    public function getIsLikedAttribute()
    {
        $like = $this->likes()->whereUserId(Auth::id())->first();
        return (!is_null($like)) ? true : false;
    }

    //User belongs to posts
    public function user()
    {
        return $this->belongsTo(User::class);
    }

    //Option for published contents
    public function setPostLiveAttribute($value)
    {
        $this->attributes['post_live'] = (boolean) ($value);
    }

    //Convert url to slug format
    public function setPostSlugAttribute($value)
    {
        if (empty($value)) {
            $title = $this->attributes['post_title'];
            $this->attributes['post_slug'] = Str::slug($title, '-');
            if (empty($this->attributes['post_slug'])) {
                $this->attributes['post_slug'] = Str::random(15);
            }
        } else {
            $this->attributes['post_slug'] = Str::slug($value, '-');
        }
    }
    //Filter for archive mounth and year
    public function scopeFilter($query, $filters)
    {
        if ($month = $filters['month']) {
            $query->whereMonth('created_at', Carbon::parse($month)->month);
        }

        if ($year = $filters['year']) {
            $query->whereYear('created_at', $year);
        }
    }
    //Get database information and convert to archive
    public static function archives()
    {
        return static::selectRaw('year(created_at) year, monthname(created_at) month, count(*) published')
            ->groupBy('year', 'month')
            ->orderByRaw('min(created_at) desc')
            ->wherePostLive(1)
            ->get()
            ->toArray();
    }
    //Categories here
    public function tags()
    {
        return $this->belongsToMany(Tag::class);
    }

    //Content function
    public function contents()
    {
        return $this->hasMany(Content::class);
    }
    
    //For firendly urls
    public function getRouteKeyName()
    {
        return 'post_slug';
    }
    //Search function
    public function scopeSearch($query, $s)
    {
        return $query
        ->where('post_title', 'like', '%' . $s . '%')
        ->orWhere('post_desc', 'like', '%' . $s . '%')
        ->orWhereHas('contents', function ($query) use ($s) {
            $query->where('body', 'like', '%' . $s . '%');
        });
    }
}
