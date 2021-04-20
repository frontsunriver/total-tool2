@extends('layouts.backend.app')

@section('content')
<div class="card">
    <div class="card-body">
        <h4>{{ __('Theme List') }}</h4>
        <div class="theme-main-area">
            <div class="row mt-50">
                @foreach($themes as $theme)
                <div class="col-lg-4 mb-30">
                    <div class="single-theme text-center">
                        <div class="theme-img">
                            <img class="img-fluid" src="{{ asset('script/am-content/Themes/'.$theme['Text Domain'].'/'.$theme['Thumbnail']) }}" alt="">
                        </div>
                        <div class="theme-footer text-center">
                            <span>{{ $theme['Theme Name'] }}</span>
                        </div>
                        <div class="theme-author-version">
                            <span>{{ __('Version') }}: {{ $theme['Version'] }}</span>
                            <span>{{ __('Author: Amcoders') }}</span>
                        </div>
                        <div class="theme-actions mt-3">
                            @if($theme['status'] == 'deactive')
                            <a href="{{ route('admin.theme.active',$theme['Text Domain']) }}"><i class="fa fa-check-circle"></i> {{ __('Active') }}</a>
                            @endif
                            <a href="{{ route('admin.customizer.index') }}"><i class="fa fa-check-circle"></i> {{ __('Customize') }}</a>
                        </div>
                    </div>
                </div>
                @endforeach
            </div>
        </div>
    </div>
</div>
<!-- Modal -->
<div class="modal fade" id="new_theme" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalCenterTitle">{{ __('Upload Theme') }}</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <form action="{{ route('admin.theme.upload') }}" method="POST" enctype="multipart/form-data">
        @csrf
        <div class="modal-body mt-35 mb-35">
         <div class="custom-form">
            <div class="custom-file">
              <input type="file" class="custom-file-input" name="file" id="theme_file">
              <label class="custom-file-label" for="theme_file">{{ __('Choose file') }}</label>
          </div>
      </div>
  </div>
  <div class="modal-footer">
    <button type="submit" class="btn">{{ __('Install Now') }}</button>
</div>
</form>
</div>
</div>
</div>
@endsection