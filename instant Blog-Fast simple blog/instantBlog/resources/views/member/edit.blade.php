@extends('layouts.master')
@section('css')
<link href="{{ asset('/summernote/summernote-bs4.css') }}" rel="stylesheet"> 
<style type="text/css">
    .bg-nav {
        margin-top: 0px!important;
        padding-top: 1.35rem;
        background: linear-gradient(to right bottom, #3f4756, #4a4a69, #634874, #834074, #a23567);
    }
</style>
@endsection
@section('bodyclass')
    <body>
@endsection
@section('jumbotron')
<div class="jumbotron jblight">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="display-4">@lang('messages.form.title_edit')</h1>
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="content">
    <div class="alert alert-success print-success-msg d-none" role="alert">
    </div>
    @canany(['own-post', 'moderator-post'], $post)
        <form method="POST" action="{{url('/home/' . $post->id)}}" id="post_form">
            {{ method_field('PUT') }}
            <div class="form-group row">
                <label for="post_title" class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.title')</label>
                <div class="col-md-7">
                    <input type="text" class="form-control" id="post_title" name="post_title" value="{{ $post->post_title }}">
                </div>
            </div>
            <div class="form-group row">
                <label for="post_slug" class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.slug')</label>
                <div class="col-md-7">
                    <input type="text" class="form-control" id="post_slug" name="post_slug" value="{{ $post->post_slug }}" >
                </div>
            </div>
            <div class="form-group row">
                <label for="post_desc" class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.description')</label>
                <div class="col-md-7">
                    <textarea class="form-control" id="post_desc" name="post_desc">{{ $post->post_desc }}</textarea>
                </div>
            </div>
            <div class="form-group row">
              <label for="post_desc" class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.type')</label>
              <div class="col-md-7">
                <ul class="nav nav-pills" id="pills-tab" role="tablist">
                  <li class="nav-item">
                    <a class="nav-link active" id="pills-image-tab" data-toggle="pill" href="#pills-image" role="tab" aria-controls="pills-image" aria-selected="true">@lang('messages.form.imagepost')</a>
                  </li>
                  <li class="nav-item">
                    <a class="nav-link" id="pills-video-tab" data-toggle="pill" href="#pills-video" role="tab" aria-controls="pills-video" aria-selected="false">@lang('messages.form.videopost')</a>
                  </li>
                  <li class="nav-item">
                    <a class="nav-link" id="pills-text-tab" data-toggle="pill" href="#pills-text" role="tab" aria-controls="pills-text" aria-selected="false">@lang('messages.form.textpost')</a>
                  </li>
                </ul>
              </div>
            </div>
            <div class="tab-content" id="pills-tabContent">
              <div class="tab-pane fade show active" id="pills-image" role="tabpanel" aria-labelledby="pills-image-tab">
                @if (!empty($post->post_media))
                <div class="form-group row">
                    <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.upload')</label>
                    <div class="col-sm-4 col-md-2 fileinputs">
                        <button type="button" class="btn btn-warning btn-block editimage">@lang('messages.form.remove')</button>
                    </div>
                    <input class="photo_upload" name="post_media" type="hidden" value="{{ $post->post_media }}">
                    <div class="col-sm-6 col-md-5 fileinfo">
                        <p><img class="imgthumb img-fluid" src="{{ url('/uploads/' . $post->post_media) }}"> @lang('messages.form.uploaded') </p>
                    </div>
                </div>
                <div class="form-group row">
                        <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.imgoverlay')</label>
                    <div class="col-sm-7">
                        <div class="form-check">
                            @if ($post->post_instant == "1")
                            <input class="form-check-input" type="checkbox" id="gridCheck1" name="post_instant" value="1" checked>
                            @else
                            <input class="form-check-input" type="checkbox" id="gridCheck1" name="post_instant" value="0">
                            @endif
                            <label class="form-check-label" for="gridCheck1">
                                @lang('messages.form.imgoverlay_check')
                            </label>
                        </div>
                    </div>
                 </div>
                @else
                <div class="form-group row">
                    <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.upload')</label>
                    <div class="col-sm-4 col-md-2 fileinputs">
                        <label class="btn btn-info btn-block btnfile">@lang('messages.form.browse')
                            <input class="fileupload d-none" type="file" name="post_image">
                        </label>
                    </div>
                    <input class="photo_upload" name="post_media" type="hidden" value="">
                    <div class="col-sm-6 col-md-5 fileinfo">
                    </div>
                </div>
                @endif
              </div>
              <div class="tab-pane fade" id="pills-video" role="tabpanel" aria-labelledby="pills-video-tab">
                <div class="form-group row">
                    <label for="post_video" class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.postvideo')</label>
                    <div class="col-md-7">
                        <input type="text" class="form-control" id="post_video" name="post_video" aria-describedby="videoHelp" value="{{ $post->video_url }}">
                        <small id="videoHelp" class="form-text text-muted">@lang('messages.form.videoex')</small>
                    </div>
                </div>
              </div>
              <div class="tab-pane fade" id="pills-text" role="tabpanel" aria-labelledby="pills-text-tab">
                <div class="form-group row">
                  <label for="post_video" class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.bg')</label>
                 <div class="col-md-7">
                                <div class="form-check form-check-inline">    
                                    <input class="form-check-input" type="radio" name="post_color" id="inlineRadio1" value="bg-primary" checked>
                                        <label class="form-check-label color-box bg-primary text-white" for="inlineRadio1"><i class="icon-list icons"></i>
                                    </label> 
                                </div>
                                <div class="form-check form-check-inline">    
                                    <input class="form-check-input" type="radio" name="post_color" id="inlineRadio2" value="bg-secondary">
                                        <label class="form-check-label color-box bg-secondary text-white" for="inlineRadio2"><i class="icon-list icons"></i>
                                    </label> 
                                </div>
                                <div class="form-check form-check-inline">    
                                    <input class="form-check-input" type="radio" name="post_color" id="inlineRadio3" value="bg-danger">
                                        <label class="form-check-label color-box bg-danger text-white" for="inlineRadio3"><i class="icon-list icons"></i>
                                    </label> 
                                </div>
                                <div class="form-check form-check-inline">    
                                    <input class="form-check-input" type="radio" name="post_color" id="inlineRadio4" value="bg-warning">
                                        <label class="form-check-label color-box bg-warning text-white" for="inlineRadio4"><i class="icon-list icons"></i>
                                    </label> 
                                </div>
                                <div class="form-check form-check-inline">    
                                    <input class="form-check-input" type="radio" name="post_color" id="inlineRadio5" value="bg-info">
                                        <label class="form-check-label color-box bg-info text-white" for="inlineRadio5"><i class="icon-list icons"></i>
                                    </label> 
                                </div>
                                <div class="form-check form-check-inline">    
                                    <input class="form-check-input" type="radio" name="post_color" id="inlineRadio6" value="bg-dark">
                                        <label class="form-check-label color-box bg-dark text-white" for="inlineRadio6"><i class="icon-list icons"></i>
                                    </label> 
                                </div>
                  </div>
                </div>
              </div>
            </div>

            @include('posts.tagselect')

            @if(auth()->user()->can('moderator-post') OR $post->post_live == 1)
            <div class="form-group row">
                <label for="post_live" class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.publish')</label>
                <div class="col-md-7">
                    <input type="checkbox" name="post_live" {{ $post->post_live == 1 ? 'checked' : '' }}>
                </div>
            </div>
            @endif

            <div id="dynamic_field">
                @foreach ($post->contents as $content)
                @if ($content->type == "image")
                <div class="form-group row">
                    <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.upload')</label>
                    <div class="col-sm-4 col-md-2 fileinputs">
                        <button type="button" class="btn btn-warning btn-block editimage">@lang('messages.form.remove')</button>
                    </div>
                    <input class="photo_upload" name="content[]" type="hidden" value="{{ $content->body }}">
                    <input type="hidden" name="type[]" value="image">
                    <input type="hidden" name="id[]" value="{{ $content->id }}">
                    <div class="col-sm-6 col-md-5 fileinfo">
                        <p><img class="imgthumb img-fluid" src="{{ url('/uploads/' . $content->body) }}"> @lang('messages.form.uploaded')</p>
                    </div>
                    <div class="col-sm-2 col-md-1">
                        <button type="button"  name="remove" class="btn btn-danger btn_del">X</button>
                    </div>
                </div>
                @elseif ($content->type == "text")
                <div class="form-group row">
                    <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.text')</label>
                    <div class="col-sm-10 col-md-7">
                        <textarea class="form-control" name="content[]">{{ $content->body }}</textarea>
                        <input type="hidden" name="type[]" value="text">
                        <input type="hidden" name="id[]" value="{{ $content->id }}">
                    </div>
                    <div class="col-sm-2 col-md-1">
                        <button type="button"  name="remove" class="btn btn-danger btn_del">X</button>
                    </div>
                </div>
                @elseif ($content->type == "txteditor")
                <div class="form-group row">
                    <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.editor')</label>
                    <div class="col-sm-10 col-md-7">
                        <textarea class="summernote" name="content[]">{!! clean( $content->body ) !!}</textarea>
                        <input type="hidden" name="type[]" value="txteditor">
                        <input type="hidden" name="id[]" value="{{ $content->id }}">
                    </div>
                    <div class="col-sm-2 col-md-1">
                        <button type="button" name="remove" class="btn btn-danger btn_del">X</button>
                    </div>
                </div>
                @elseif ($content->type == "youtube")
                    <div class="form-group row">
                        <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.youtube')</label>
                        <div class="col-sm-10 col-md-7">
                            <input type="text" class="form-control" name="content[]" value="https://www.youtube.com/watch?v={{ $content->body }}">
                            <input type="hidden" name="type[]" value="youtube">
                            <input type="hidden" name="id[]" value="{{ $content->id }}">
                        </div>
                        <div class="col-sm-2 col-md-1">
                            <button type="button" name="remove" class="btn btn-danger btn_del">X</button>
                        </div>
                    </div>
                @elseif (!empty($content->embed_id))
                <div class="form-group row">
                    <label class="offset-md-1 col-md-2 col-form-label">{{ title_case($content->type) }}</label>
                    <div class="input-group col-sm-10 col-md-7">                
                        <input name="embed_url" type="text" class="form-control" value="{{ $content->embed->url }}" readonly>
                        <div class="input-group-addon">
                            <i class="icon-check icons text-success"></i>
                        </div>
                        <input type="hidden" class="form-control" name="content[]" value="{{ $content->body }}">
                        <input type="hidden" name="type[]" value="{{ $content->type }}">
                        <input type="hidden" name="id[]" value="{{ $content->id }}">
                        <span class="input-group-btn">                            
                            <button type="button" class="btn btn-warning remove_embed">@lang('messages.form.remove')</button>
                        </span>
                    </div>
                    <div class="col-sm-2 col-md-1">
                        <button type="button" name="remove" class="btn btn-danger btn_del">X</button>
                    </div>
                </div>
                @else
                <div class="form-group row">
                    <label class="offset-md-1 col-md-2 col-form-label">{{ title_case($content->type) }}</label>
                    <div class="col-sm-10 col-md-7">
                        <input type="text" class="form-control" name="content[]" value="{{ $content->body }}">
                        <input type="hidden" name="type[]" value="{{ $content->type }}">
                        <input type="hidden" name="id[]" value="{{ $content->id }}">
                    </div>
                    <div class="col-sm-2 col-md-1">
                        <button type="button" name="remove" class="btn btn-danger btn_del">X</button>
                    </div>
                </div>
                @endif
                @endforeach
            </div>

            <div class="form-group row mb-5">
                    <label class="offset-md-1 col-md-2 col-form-label"><strong>@lang('messages.form.more') </strong> <a href="#" data-toggle="modal" data-target="#helpModal"><i class="icon-question icons"></i></a>
                    </label>
                    <div class="col-md-7">             
                        <button type="button" name="header_add" class="btn btn-lg btn-light btnadd mr-1" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addheading')">H</button>
                        <button type="button" name="txt_add" class="btn btn-lg btn-light btnadd mr-1" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addsimple')">T</button>
                        <button id="texteditor" type="button" class="btn btn-lg btn-light mr-1" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addeditor')">T+</button>
                        <button type="button" name="img_add" class="btn btn-lg btn-light btnadd mr-1" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addimage')"><i class="icon-picture icons"></i></button>
                        <button type="button" name="youtube_add" class="btn btn-lg btn-light btnadd mr-1" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addyoutube')"><i class="icon-social-youtube icons"></i></button>
                        <button type="button" name="tweet_add" class="btn btn-lg btn-light btnadd mr-1" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addtweet')"><i class="icon-social-twitter icons"></i></button>
                        <button type="button" name="face_add" class="btn btn-lg btn-light btnadd mr-1" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addfb')"><i class="icon-social-facebook icons"></i></button>
                        <button type="button" name="instagram_add" class="btn btn-lg btn-light btnadd" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addinst')"><i class="icon-social-instagram icons"></i></button>
                        <button type="button" name="pinterest_add" class="btn btn-lg btn-light btnadd" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.form.addpin')"><i class="icon-social-pinterest icons"></i></button>
                    </div>
            </div>
            <div id="subbtn" class="form-group row mb-5">
                <div class="offset-md-3 col-md-7">
                    <div class="text-danger print-error-msg d-none">
                        <ul></ul>
                    </div>             
                    <input type="button" name="submit" id="submit" class="btn btn-success" value="@lang('messages.form.update')" />
                </div>
            </div>
        </form>
        @include('posts.formfields')
    @else
    <div class="alert alert-warning" role="alert">
      <strong>@lang('messages.form.nofound')</strong> @lang('messages.backto') <a href="{{ url('/') }}">@lang('messages.hometxt')</a>.
    </div>
    @endcanany
    </div>
</div>
@endsection
@push('scripts')
<script src="{{ asset('/summernote/summernote-bs4.js') }}"></script>
  <script type="text/javascript">
    $(document).ready(function() {
    $('.summernote').summernote({
        height: 100,
        disableDragAndDrop: true,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['para', ['ul', 'ol']],
            ['mis', ['link']],   
        ],
        callbacks: {
            onPaste: function (e) {
                var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');
                e.preventDefault();
                setTimeout( function(){
                    document.execCommand( 'insertText', false, bufferText );
            }, 10 );
        }
        }
    });
    });
  </script>
  <script type="text/javascript">
    var embedURL = "{{ url('admincp/postEmbed') }}";
    var imgURL = "{{ url('admincp/uploadImg') }}";
    var delURL = "{{ url('admincp/deleteImg') }}";
    var avatarURL = "{{ url('/uploads/') }}";
    var delContent = "{{ url('/delete/content') }}";
    var embedtxt = "@lang('messages.form.embed')";
    var editortxt = "@lang('messages.form.editor')";
    var removetxt = "@lang('messages.form.removetxt')";
    var processing = "@lang('messages.form.processing')";
    var submittxt = "@lang('messages.form.submittxt')";
    var browse = "@lang('messages.form.browse')";
    var imguploaded = "@lang('messages.form.imguploaded')";
    var imgremoved = "@lang('messages.form.imgremoved')";
    var fileUploading = "@lang('messages.form.file_uploading')";
  </script>
  <script src="{{ asset('js/form.js') }}"></script>
@endpush