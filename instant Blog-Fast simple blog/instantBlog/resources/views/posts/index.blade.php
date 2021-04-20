@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-9">
        <h1 class="display-4">Posts</h1>
    </div>
    <div class="col-3">
        <a class="btn btn-primary" href="{{ url('/home/add/') }}" role="button"><i class="icon-plus icons"></i> Add New Post</a>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="row ml-3 mr-3">
        <ul class="nav nav-tabs">
            <li class="nav-item">
                <a class="nav-link active" href="{{ url('/contents/') }}">Published</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="{{ url('/unpublished/') }}">Unpublished</a>
            </li>
        </ul>
    </div>  
    <div class="row box-white ml-3 mr-3">                    
        <table class="table table-sm table-hover">
            <thead>
                <tr>
                    <th><input type="checkbox" id="checkAll"/></th>
                    <th>Title</th>
                    <th>User</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <form action="{{ url('/cnt/multiple/') }}" method="POST">
                    @csrf
                    {{ method_field('POST') }}
            <tbody>
                @forelse($posts as $post)      
                @include('posts.post')
                @empty
                <h5>
                    No Posts Found
                </h5>
                @endforelse
            </tbody>
        </table>
                <button name="mulbtn"  type="submit" class="btn btn-danger btn-sm" value="Delete">Delete</button>
                <button name="mulbtn"  type="submit" class="btn btn-secondary btn-sm ml-2" value="Unpublish">Unpublish</button>
            </form>
    </div>
    <div class="row">
        <div class="col-md-12">
            {{ $posts->links() }}
        </div>
    </div> 
</div>
@endsection
