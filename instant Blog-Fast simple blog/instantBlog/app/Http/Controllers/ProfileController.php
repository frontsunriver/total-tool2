<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Validation\Rule;
use Illuminate\Support\Facades\Auth;
use Image;
use App\Models\User;
use App\Models\Post;

class ProfileController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth', ['except' => ['profile']]);
    }
    
    public function profile($username)
    {
        $user = User::whereUsername($username)->firstOrFail();
        
        $posts = Post::latest()
            ->wherePostLive(1)
            ->whereUserId($user->id)
            ->paginate(30);

        $userlikes = Post::latest()
            ->wherePostLive(1)
            ->whereUserId($user->id)
            ->withCount('likes')
            ->take(5)
            ->get();

        $point = Post::wherePostLive(1)
            ->select('user_id')
            ->whereUserId($user->id)
            ->withCount('likes')
            ->get();

        return view('member.profile', compact('user', 'posts', 'userlikes', 'point'));
    }

    public function edit($username)
    {
        $user = User::findOrFail(auth()->id());

        $point = Post::wherePostLive(1)
            ->select('user_id')
            ->whereUserId($user->id)
            ->withCount('likes')
            ->get();
            
        return view('member.profileedit', compact('user', 'point'));
    }

    public function update(Request $request, $id)
    {
        $user = User::findOrFail($id);

        $attributes = request(['name',  'username', 'avatar', 'email', 'website' , 'facebook' ,
        'twitter', 'instagram', 'linkedin']);

        if($request->password) {
            $this->validate(request(), [
                'password' => 'required|min:6|confirmed',
            ]);

            $user->password = bcrypt(request('password'));
            $user->save();

        } else {

        $this->validate(request(), [
            'name' => 'required|max:255',
            'username' => [
                'required',
                Rule::unique('users')->ignore($user->id),
            ],
            'email' => [
                'required',
                Rule::unique('users')->ignore($user->id),
            ],
        ]);

        if ($request->hasFile('avatar')) {
            $postimage = $request->file('avatar');
            $filename = time() . '.' . $postimage->getClientOriginalExtension();
            Image::make($postimage)->resize(100, 100)->save(public_path('/images/'. $filename));
            $attributes['avatar'] = $filename;
        } else {
            $attributes['avatar'] = $user->avatar ;
        }

            $user->update($attributes);
        }

        session()->flash('message', 'Profile Updated!');

        return redirect('/home');
    }
}
