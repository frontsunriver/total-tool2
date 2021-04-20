@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-9">
        <h1 class="display-4">Edit Admin Profile</h1>
           <label class="badge badge-light">@lang($admin->role)</label>
    </div>
    <div class="col-3">
        <div class="inbox-item">
            <div class="admin-item-img">
                <img src="{{ url('/images/' . $admin->avatar) }}" class="admin-image rounded-circle">
            </div>
            <p class="admin-item-user">{{ $admin->name }}</p>
            <p class="admin-item-text">{{ $admin->username }}</p>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    @can('admin-secret')
    <div class="box-white ml-3 mr-3">   
        @include('layouts.errors')
        <form method="POST" action="{{url('/adminprofile/' . $admin->id)}}" enctype="multipart/form-data">
            {{ method_field('PUT') }}
            @csrf
            <div class="form-group row">
                <label for="name" class="col-sm-4 col-form-label">Name</label>
                <div class="col-sm-7">
                    <input type="text" class="form-control" id="name" name="name" value="{{ $admin->name }}" required>
                </div>
            </div>
            <div class="form-group row">
                <label for="username" class="col-sm-4 col-form-label">Username</label>
                <div class="col-sm-7">
                    <input type="text" class="form-control" id="username" name="username" value="{{ $admin->username }}" required>
                </div>
            </div>
            <div class="form-group row">
                <label for="email"  class="col-sm-4 col-form-label">Email:</label>
                <div class="col-sm-7">
                    <input type="email" class="form-control" id="email" name="email" value="{{ $admin->email }}" required>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label" >Admin image</label>
                <div class="col-sm-6">     
                    <input type="file" class="form-control-file" id="avatar" name="avatar" aria-describedby="fileHelp">
                    <small id="fileHelp" class="form-text text-muted">Choose File if you like to add new image or update exiting.</small>
                </div>
            </div>
            <div class="form-group row">
                <label for="website" class="col-sm-4 col-form-label">Website Url :</label>
                <div class="col-sm-7">
                    <input type="text" class="form-control" id="website" name="website" value="{{ $admin->website }}">
                </div>
            </div>
            <div class="form-group row">
                <label for="facebook" class="col-sm-4 col-form-label">Facebook :</label>
                <div class="col-sm-7">
                    <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                        <div class="input-group-addon">facebook.com/</div>
                        <input type="text" class="form-control" id="facebook" name="facebook" value="{{ $admin->facebook }}">
                    </div>                            
                </div>
            </div>
            <div class="form-group row">
                <label for="twitter" class="col-sm-4 col-form-label">Twitter :</label>
                <div class="col-sm-7">
                    <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                        <div class="input-group-addon">twitter.com/</div>
                        <input type="text" class="form-control" id="twitter" name="twitter" value="{{ $admin->twitter }}">
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <label for="instagram" class="col-sm-4 col-form-label">Instagram :</label>
                <div class="col-sm-7">
                    <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                        <div class="input-group-addon">instagram.com/</div>
                        <input type="text" class="form-control" id="instagram" name="instagram" value="{{ $admin->instagram }}">
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <label for="linkedin" class="col-sm-4 col-form-label">LinkedIn :</label>
                <div class="col-sm-7">
                    <input type="text" class="form-control" id="linkedin" name="linkedin" value="{{ $admin->linkedin }}">
                </div>
            </div>
            <div class="form-group row">
                <div class="offset-sm-4 col-sm-7">
                    <button type="submit" class="btn btn-primary">Save</button>
                    <a href="{{ url('/home/') }}" class="btn btn-danger" role="button">Cancel</a> 
                </div>
            </div>
        </form>
    </div>
    <div class="box-white ml-3 mr-3">
        <form method="POST" action="{{url('/adminprofile/' . $admin->id)}}" enctype="multipart/form-data">
            {{ method_field('PUT') }}
            @csrf
            <div class="form-group row">
                <label for="password"  class="col-sm-4 col-form-label">Password:</label>
                <div class="col-sm-7">
                    <input type="password" class="form-control" id="password" name="password" required>
                </div>
            </div>
            <div class="form-group row">
                <label for="password_confirmation"  class="col-sm-4 col-form-label">Password Confirmation:</label>
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
    </div>
    @endcan
</div>
@endsection