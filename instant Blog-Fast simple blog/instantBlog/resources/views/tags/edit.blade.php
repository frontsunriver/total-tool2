@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-12">
        <h1 class="display-4">Edit Category</h1>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="box-white ml-3 mr-3">
        @include('layouts.errors')
        <form method="POST" action="{{url('/cats/' . $tag->id)}}" enctype="multipart/form-data">
            {{ method_field('PUT') }}
            @csrf
            <div class="form-group row">
                <label for="title" class="col-sm-4 col-form-label">Category Title</label>
                <div class="col-sm-7">
                    <input type="text" class="form-control" id="title" name="title" value="{{ $tag->title }}" required>
                </div>
            </div>
            <div class="form-group row">
                <label for="name" class="col-sm-4 col-form-label">Category Name</label>
                <div class="col-sm-7">
                    <input type="text" class="form-control" id="name" name="name" value="{{ $tag->name }}" required>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label" >File input</label>
                <div class="col-sm-1">
                    <img class="avatar img-fluid" src="{{ url('/uploads/' . $tag->tag_media) }}">
                </div> 
                <div class="col-sm-6">     
                    <input type="file" class="form-control-file" id="tag_media" name="tag_media" aria-describedby="fileHelp">
                    <small id="fileHelp" class="form-text text-muted">Choose file if you like to update exiting image.</small>
                </div>
            </div>
            <div class="form-group row">
                <label for="desc" class="col-sm-4 col-form-label">Meta Description</label>
                <div class="col-sm-7">
                    <input type="text" class="form-control" id="desc" name="desc" value="{{ $tag->desc }}">
                    <small id="desc" class="form-text text-muted">This is meta description for seo (optional) .</small>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label" >Category Color</label>
                <div class="col-sm-6"> 
                    <div class="form-check form-check-inline mt-2">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio1" value="bg-cat-1" checked>
                        <label class="form-check-label color-box bg-cat-1 text-white" for="inlineRadio1"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline mt-2">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio2" value="bg-cat-4" >
                        <label class="form-check-label color-box bg-cat-4 text-white" for="inlineRadio2"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio3" value="bg-danger">
                        <label class="form-check-label color-box bg-danger text-white" for="inlineRadio3"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline mt-2">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio4" value="bg-cat-2" >
                        <label class="form-check-label color-box bg-cat-2 text-white" for="inlineRadio4"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio5" value="bg-primary" >
                        <label class="form-check-label color-box bg-primary text-white" for="inlineRadio5"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio6" value="bg-info">
                        <label class="form-check-label color-box bg-info text-white" for="inlineRadio6"><i class="icon-list icons"></i>
                        </label> 
                    </div>                    
                    <div class="form-check form-check-inline">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio7" value="bg-success">
                        <label class="form-check-label color-box bg-success text-white" for="inlineRadio7"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio8" value="bg-warning">
                        <label class="form-check-label color-box bg-warning text-white" for="inlineRadio8"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline mt-2">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio9" value="bg-cat-3" >
                        <label class="form-check-label color-box bg-cat-3 text-white" for="inlineRadio9"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline mt-2">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio10" value="bg-cat-5" >
                        <label class="form-check-label color-box bg-cat-5 text-white" for="inlineRadio10"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio11" value="bg-secondary" >
                        <label class="form-check-label color-box bg-secondary text-white" for="inlineRadio11"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                    <div class="form-check form-check-inline">    
                        <input class="form-check-input" type="radio" name="color" id="inlineRadio12" value="bg-dark">
                        <label class="form-check-label color-box bg-dark text-white" for="inlineRadio12"><i class="icon-list icons"></i>
                        </label> 
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <div class="offset-sm-4 col-sm-7">
                    <button type="submit" class="btn btn-primary">Update</button>
                </div>
            </div>
        </form>
    </div>
</div>
@endsection