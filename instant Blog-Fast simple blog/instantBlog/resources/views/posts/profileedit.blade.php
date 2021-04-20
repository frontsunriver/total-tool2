@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-12">
        <h1 class="display-4">Edit User Profile</h1>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="row">
        <div class="col-lg-3 col-lg-pull-6 col-md-6">
            <div class="profile-card">
                <div>
                    @if (substr( $user->avatar, 0, 4 ) === "http")
                    <img class="profile-card-photo" src=" {{ $user->avatar }} ">
                    @else
                    <img class="profile-card-photo" src="{{ url('/images/' . $user->avatar) }}">
                    @endif
                </div>
                <div class="profile-card-name">{{ $user->name }}</div>
                <div class="profile-card-username">{{ $user->username }}</div>
                @isset($user->role)
                <label class="badge badge-info">{{ $user->role }}</label>
                @endisset
            </div>
        </div>
        <div class="col-lg-9 col-lg-push-3 col-md-6">
            <section class="box-white">
                @include('layouts.errors')
                <form method="POST" action="{{url('/users/' . $user->id)}}" enctype="multipart/form-data">
                    {{ method_field('PUT') }}
                    @csrf
                    <div class="form-group row">
                        <label for="name" class="col-sm-4 col-form-label">Name :</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="name" name="name" value="{{ $user->name }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="username" class="col-sm-4 col-form-label">Username :</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="username" name="username" value="{{ $user->username }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="rolechange" class="col-sm-4 col-form-label">Role :</label>
                        <div class="col-sm-7">
                            <select class="custom-select" name="rolechange">
                                <option value="normal">user</option>
                                <option value="verified" {{ ( $user->role == 'verified') ? 'selected' : '' }}>verified</option>
                                <option value="editor" {{ ( $user->role == 'editor') ? 'selected' : '' }}>editor</option>
                                <option value="admin" {{ ( $user->role == 'admin') ? 'selected' : '' }}>admin</option>
                            </select>
                         </div>
                    </div>
                    <div class="form-group row">
                        <label for="email"  class="col-sm-4 col-form-label">Email :</label>
                        <div class="col-sm-7">
                            <input type="email" class="form-control" id="email" name="email" value="{{ $user->email }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                    <label class="col-sm-4 col-form-label" >User image</label>
                        <div class="col-sm-6">     
                            <input type="file" class="form-control-file" id="avatar" name="avatar" aria-describedby="fileHelp">
                            <small id="fileHelp" class="form-text text-muted">Choose File if you like to add new image or update exiting.</small>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="website" class="col-sm-4 col-form-label">Website Url :</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="website" name="website" value="{{ $user->website }}">
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="facebook" class="col-sm-4 col-form-label">Facebook :</label>
                        <div class="col-sm-7">
                            <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                                <div class="input-group-addon">facebook.com/</div>
                                <input type="text" class="form-control" id="facebook" name="facebook" value="{{ $user->facebook }}">
                            </div>                            
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="twitter" class="col-sm-4 col-form-label">Twitter :</label>
                        <div class="col-sm-7">
                            <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                                <div class="input-group-addon">twitter.com/</div>
                                <input type="text" class="form-control" id="twitter" name="twitter" value="{{ $user->twitter }}">
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="instagram" class="col-sm-4 col-form-label">Instagram :</label>
                        <div class="col-sm-7">
                            <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                                <div class="input-group-addon">instagram.com/</div>
                                <input type="text" class="form-control" id="instagram" name="instagram" value="{{ $user->instagram }}">
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="linkedin" class="col-sm-4 col-form-label">LinkedIn :</label>
                        <div class="col-sm-7">
                                <input type="text" class="form-control" id="linkedin" name="linkedin" value="{{ $user->linkedin }}">
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="offset-sm-4 col-sm-7">
                            <button type="submit" class="btn btn-primary">Save</button>
                            <a href="{{ url('/home/') }}" class="btn btn-danger" role="button">Cancel</a> 
                        </div>
                    </div>
                </form>
                 </section>
                <section class="box-white">
                <form method="POST" action="{{url('/users/' . $user->id)}}" enctype="multipart/form-data">
                    {{ method_field('PUT') }}
                    @csrf
                    <div class="form-group row">
                        <label for="password"  class="col-sm-4 col-form-label">Password :</label>
                        <div class="col-sm-7">
                            <input type="password" class="form-control" id="password" name="password" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="password_confirmation"  class="col-sm-4 col-form-label">Password Confirmation :</label>
                        <div class="col-sm-7">
                            <input type="password" class="form-control" id="password_confirmation" name="password_confirmation" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="offset-sm-4 col-sm-7">
                            <button type="submit" class="btn btn-primary">Change Password</button>
                        </div>
                    </div>
                </form>
            </section>
        </div>        
    </div>
</div>
@endsection