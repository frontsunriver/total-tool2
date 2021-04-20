@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-12">
        <h1 class="display-4">Users</h1>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="row box-white ml-3 mr-3">
        <table class="table table-sm table-hover">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Username</th>
                    <th>Role</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                @forelse($users as $user)      
                <tr> 
                    <th scope="row">{{ $user->id }}</th>
                    <td>
                        <a href="{{url('/profile/' . $user->username)}}">
                            {{ $user->name }}
                        </a>
                    </td>
                    <td>{{ $user->username}}</td>
                    <td>
                    @isset($user->role)
                    <label class="badge badge-info">{{ $user->role }}</label>
                    @endisset
                    </td>
                    <td><a href="{{ url('/users/' . $user->username . '/edit') }}">Edit</a></td>
                    <td><a class="color-delete" href="{{ url('/users/' . $user->username) }}">Delete</a></td>
                </tr>
                @empty
                No Users Found
                @endforelse
            </tbody>
        </table>
    </div>
    <div class="row">
        <div class="col-md-12">
            {{ $users->links() }}
        </div>
    </div>
</div>
@endsection