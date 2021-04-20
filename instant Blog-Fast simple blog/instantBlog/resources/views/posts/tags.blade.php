@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-9">
        <h1 class="display-4">Categories</h1>
    </div>
    <div class="col-3">
        <a class="btn btn-primary" href="{{ url('/cats/create/') }}" role="button"><i class="icon-plus icons"></i> Add Category</a>
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
                    <th>Title</th>
                    <th>Name</th>
                    <th>Date</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                @forelse($tags as $tag)      
                <tr> 
                    <th scope="row">{{ $tag->id }}</th>
                    <td>
                        <a href="{{url('/category/' . $tag->name)}}">
                            {{ str_limit($tag->title, 35) }}
                        </a>
                    </td>
                    <td><div class="cat-bx {{ $tag->color}}"></div> {{ $tag->name}}</td>
                    <td>{{ $tag->created_at->diffForHumans() }}</td>
                    <td><a href="{{ url('/cats/' . $tag->id . '/edit') }}">Edit</a></td>
                    <td><a class="color-delete" href="{{ url('/cats/' . $tag->id) }}">Delete</a></td>
                </tr>
                @empty
                No Posts
                @endforelse
            </tbody>
        </table>
    </div>
    <div class="row">
        <div class="col-md-12">
            {{ $tags->links() }}
        </div>
    </div>
</div>
@endsection