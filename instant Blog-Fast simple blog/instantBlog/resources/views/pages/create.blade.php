@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-12">
        <h1 class="display-4">Add Page</h1>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="box-white ml-3 mr-3">
        @include('layouts.errors')
        <form method="POST" action="{{ url('/pages') }}" enctype="multipart/form-data">
            @csrf
            <div class="form-group row">
                <label for="page_title" class="col-sm-3 col-form-label">Page Title</label>
                <div class="col-sm-9">
                    <input type="text" class="form-control" id="page_title" name="page_title" required>
                    <small id="name" class="form-text text-muted"><em>(Required)</em> - Write title for the page. Example : My page.</small>
                </div>
            </div>
            <div class="form-group row">
                <label for="page_slug" class="col-sm-3 col-form-label">Page Slug</label>
                <div class="col-sm-9">
                    <input type="text" class="form-control" id="page_slug" name="page_slug" required>
                    <small id="name" class="form-text text-muted"><em>(Required)</em> - Write slug for the page. Example : this-is-my-page.</small>
                </div>
            </div>
            <div class="form-group row">
                <label for="page_content" class="col-sm-3 col-form-label">Page Content</label>
                <div class="col-sm-9">
                <textarea id="pagenote" name="page_content"></textarea>
                </div>
            </div>
            <div class="form-group row">
                <div class="offset-sm-4 col-sm-9">
                    <button type="submit" class="btn btn-primary">Create</button>
                </div>
            </div>
        </form>
    </div>
</div>
@endsection