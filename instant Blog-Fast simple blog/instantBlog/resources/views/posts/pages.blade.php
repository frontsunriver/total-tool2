@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-9">
        <h1 class="display-4">Pages</h1>
    </div>
    <div class="col-3">
        <a class="btn btn-primary" href="{{ url('/pages/create/') }}" role="button"><i class="icon-plus icons"></i> Add Page</a>
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
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                @forelse($pages as $page)      
                <tr> 
                    <th scope="row">{{ $page->id }}</th>
                    <td>
                        <a href="{{url('/page/' . $page->page_slug)}}">
                            {{ str_limit($page->page_title, 35) }}
                        </a>
                    </td>
                    <td><a href="{{ url('/pages/' . $page->id . '/edit') }}">Edit</a></td>
                    <td><a class="color-delete" href="{{ url('/pages/' . $page->id) }}">Delete</a></td>
                </tr>
                @empty
                No Pages
                @endforelse
            </tbody>
        </table>
    </div>
    <div class="row">
        <div class="col-md-12">
            {{ $pages->links() }}
        </div>
    </div>
</div>
@endsection