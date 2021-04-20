<div class="loading">
    <div class="fusion-slider-loading">
    </div>
</div>  

<form action="{{ route('admin.customizer.image_upload') }}" id="img_form" method="POST" enctype="multipart/form-data">
    @csrf
    <div class="header-top-info single_header d-flex">
        <span  onclick="arrow()" class="arrow_left"><i class="fas fa-chevron-left"></i></span>
        <span>{{ $option['name'] }}</span>
    </div>
    <input type="hidden" id="value_update" value="{{ route('admin.customizer.value_update') }}">
    <div class="main-sidebar-area">
        @if($option['settings'] != null)
        <div class="sidebar-header-title">
            <span>Settings</span>
        </div>

        <div class="custom-form">
            @foreach($option['settings'] as $settings)
            @if($settings['type'] == 'text')
            <div class="form-group">
                <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                <input type="text" class="form-control" id="{{ $settings['id'] }}" onchange="settings_option('{{ $settings['id'] }}','text','key','{{ $option['id'] }}')" oninput="settings_option1('{{ $settings['id'] }}','text','key','{{ $option['id'] }}')" value="{{ content($option['id'],$settings['id']) }}">
            </div>
            @endif
            @if($settings['type'] == 'link')
            <div class="form-group">
                <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                <input type="text" class="form-control" id="{{ $settings['id'] }}" onchange="settings_option('{{ $settings['id'] }}','link','key','{{ $option['id'] }}')" oninput="settings_option1('{{ $settings['id'] }}','link','key','{{ $option['id'] }}')" value="{{ content($option['id'],$settings['id']) }}">
            </div>
            @endif
            @if($settings['type'] == 'icon')
            <div class="form-group">
                <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                <input type="text" class="form-control" id="{{ $settings['id'] }}" onchange="settings_option('{{ $settings['id'] }}','icon','key','{{ $option['id'] }}')" oninput="settings_option1('{{ $settings['id'] }}','icon','key','{{ $option['id'] }}')" value="{{ content($option['id'],$settings['id']) }}">
            </div>
            @endif
            @if($settings['type'] == 'textarea')
            <div class="form-group">
                <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                <textarea name="textarea" id="{{ $settings['id'] }}" cols="30" rows="5" class="form-control" onchange="settings_option('{{ $settings['id'] }}','text','key','{{ $option['id'] }}')" oninput="settings_option1('{{ $settings['id'] }}','textarea','key','{{ $option['id'] }}')">{{ content($option['id'],$settings['id']) }}</textarea>
            </div>
            @endif
            @if($settings['type'] == 'image')
            <div class="form-group imgUp">

                <label  for="{{ $settings['id'] }}">{{ $settings['label'] }}<br>

                    <div class="custom-image-area imagePreview" style="background-image: url({{ asset(content($option['id'],$settings['id'])) }});">

                        <div class="custom-img-info text-center">
                            <span>Select Image</span>
                        </div>
                    </div>
                </label>
                <input type="hidden" id="image_upload_url" value="{{ route('admin.customizer.image_upload',$settings['id']) }}">
                <input type="file" id="{{ $settings['id'] }}" name="img{{ $settings['id'] }}" class="uploadFile d-none" onchange="loadFile(event,'{{ $settings['id'] }}','image','key','{{ $option['id'] }}')">

            </div>
            @endif
             @if($settings['type'] == 'bg_image')
            <div class="form-group imgUp">

                <label  for="{{ $settings['id'] }}">{{ $settings['label'] }}<br>

                    <div class="custom-image-area imagePreview" style="background-image: url({{ asset(content($option['id'],$settings['id'])) }});">

                        <div class="custom-img-info text-center">
                            <span>Select Image</span>
                        </div>
                    </div>
                </label>
                <input type="hidden" id="image_upload_url" value="{{ route('admin.customizer.image_upload',$settings['id']) }}">
                <input type="file" id="{{ $settings['id'] }}" name="img{{ $settings['id'] }}" class="uploadFile d-none" onchange="loadFile(event,'{{ $settings['id'] }}','bg_image','key','{{ $option['id'] }}')">

            </div>
            @endif
            @endforeach

        </div>
        @endif
        @if($option['content'] != null)

        <div class="content-area">
            <div class="sidebar-header-title pb-15">
                <span>Content</span>
            </div>
            <div class="accordion" id="accordionExample">
                @foreach($option['content'] as $content)
                <div class="card">
                    <div class="card-header" id="headingOne">
                        <h2 class="mb-0">
                            <a href="#" data-toggle="collapse" data-target="#{{ $content['id'] }}" aria-expanded="true" aria-controls="{{ $content['id'] }}">
                                <span class="flaticon-squares"></span> {{ $content['name'] }}
                            </a>
                        </h2>
                    </div>

                    <div id="{{ $content['id'] }}" class="collapse" aria-labelledby="headingOne" data-parent="#accordionExample">
                        <div class="card-body">
                            <div class="custom-form">
                                @foreach($content['settings'] as $settings)
                                @if($settings['type'] == 'text')
                                <div class="form-group">
                                    <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                                    <input type="text" class="form-control" id="{{ $settings['id'] }}" onchange="section_multi_options1('{{ $settings['id'] }}','text','content','{{ $option['id'] }}','{{ $content['id'] }}')" oninput="section_multi_options('{{ $settings['id'] }}','text','content','{{ $option['id'] }}')" value="{{ content($option['id'],$settings['id'],$content['id']) }}">
                                </div>
                                @endif
                                @if($settings['type'] == 'link')
                                <div class="form-group">
                                    <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                                    <input type="text" class="form-control" id="{{ $settings['id'] }}" onchange="section_multi_options1('{{ $settings['id'] }}','link','content','{{ $option['id'] }}')" oninput="section_multi_options('{{ $settings['id'] }}','link','content','{{ $option['id'] }}')" value="{{ content($option['id'],$settings['id'],$content['id']) }}">
                                </div>
                                @endif
                                @if($settings['type'] == 'icon')
                                <div class="form-group">
                                    <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                                    <input type="text" class="form-control" id="{{ $settings['id'] }}" onchange="section_multi_options1('{{ $settings['id'] }}','icon','content','{{ $option['id'] }}')" oninput="section_multi_options('{{ $settings['id'] }}','icon','content','{{ $option['id'] }}')" value="{{ content($option['id'],$settings['id'],$content['id']) }}">
                                </div>
                                @endif
                                @if($settings['type'] == 'image')
                                <div class="form-group imgUp">

                                    <label  for="{{ $settings['id'] }}">{{ $settings['label'] }}<br>

                                        <div class="custom-image-area imagePreview" style="background-image: url({{ asset(content($option['id'],$settings['id'],$content['id'])) }});">

                                            <div class="custom-img-info text-center">
                                                <span>Select Image</span>
                                            </div>
                                        </div>
                                    </label>
                                    <input type="hidden" id="image_upload_url" value="{{ route('admin.customizer.image_upload',$settings['id']) }}">
                                    <input type="file" id="{{ $settings['id'] }}" name="img{{ $settings['id'] }}" class="uploadFile d-none" onchange="loadFile(event,'{{ $settings['id'] }}','image','content','{{ $option['id'] }}','{{ $content['id'] }}')">

                                </div>
                                @endif

                                @if($settings['type'] == 'textarea')
                                <div class="form-group">
                                    <label for="{{ $settings['id'] }}">{{ $settings['label'] }}</label>
                                    <textarea name="textarea" id="{{ $settings['id'] }}" cols="30" rows="5" class="form-control" onchange="section_multi_options1('{{ $settings['id'] }}','text','content','{{ $option['id'] }}','{{ $content['id'] }}')" oninput="section_multi_options('{{ $settings['id'] }}','textarea','content','{{ $option['id'] }}')">{{ content($option['id'],$settings['id'],$content['id']) }}</textarea>
                                </div>
                                @endif
                                @endforeach
                                <input type="hidden" id="multiple_settings_option" value="{{ route('admin.customizer.multiple_settings_option') }}">
                            </div>
                        </div>
                    </div>
                </div>
                @endforeach
            </div>
            @endif
        </div>
    </div>
</form>

