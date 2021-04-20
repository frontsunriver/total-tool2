@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-md-12">
        <h1 class="display-4">Edit Settings</h1>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="row ml-3 mr-3">
        <ul class="nav nav-tabs" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" data-toggle="tab" href="#mainsettings" role="tab">Main</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#optional" role="tab">Optional</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#adsense" role="tab">Adsense</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#facebook" role="tab">Facebook Instant Articles</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#google" role="tab">Google AMP</a>
            </li>
        </ul>
    </div>
    <div class="box-white ml-3 mr-3">  
        @include('layouts.errors')
        <form method="POST" action="{{url('/settings/' . $setting->id)}}" enctype="multipart/form-data">

            {{ method_field('PUT') }}

            @csrf
            <div class="tab-content">
                <div class="tab-pane active" id="mainsettings" role="tabpanel">
                    <div class="form-group row">
                        <label for="site_name" class="col-sm-4 col-form-label">Site Name</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="site_name" name="site_name" value="{{ $setting->site_name }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="site_desc" class="col-sm-4 col-form-label">Site Describtion</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="site_desc" name="site_desc" value="{{ $setting->site_desc }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="site_title" class="col-sm-4 col-form-label">Site Title</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="site_title" name="site_title" value="{{ $setting->site_title }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label" >Site Logo</label>
                        <div class="col-sm-6">     
                            <input type="file" class="form-control-file" id="site_logo" name="site_logo" aria-describedby="fileHelp">
                            <small id="fileHelp" class="form-text text-muted">Choose File if you like to add new logo or update exiting. Max height is 35px.</small>
                        </div>
                    </div>
                     <div class="form-group row">
                        <label for="site_title" class="col-sm-4 col-form-label">Site Footer</label>
                        <div class="col-sm-7">
                            <textarea id="summernote" name="footer">{{ $setting->footer }}</textarea>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="allow_users" class="col-sm-4 col-form-label">Allow users to add posts?</label>
                        <div class="col-sm-7">
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input" 
                                    @if ($setting->allow_users == '0')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="allow_users" id="inlineRadio1" value="0"> Yes
                                </label>
                            </div>
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input"
                                    @if ($setting->allow_users == '1')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="allow_users" id="inlineRadio2" value="1"> No
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="check_cont" class="col-sm-4 col-form-label">Check users content's before publish?</label>
                        <div class="col-sm-7">
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input" 
                                    @if ($setting->check_cont == '0')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="check_cont" id="inlineRadio1" value="0"> Yes
                                </label>
                            </div>
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input"
                                    @if ($setting->check_cont == '1')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="check_cont" id="inlineRadio2" value="1"> No
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="allow_comments" class="col-sm-4 col-form-label">Allow comments on posts?</label>
                        <div class="col-sm-7">
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input" 
                                    @if ($setting->allow_comments == '0')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="allow_comments" id="inlineRadio1" value="0"> Yes
                                </label>
                            </div>
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input"
                                    @if ($setting->allow_comments == '1')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="allow_comments" id="inlineRadio2" value="1"> No
                                </label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="tab-pane" id="optional" role="tabpanel">

                    <div class="form-group row">
                        <label for="site_analytic" class="col-sm-4 col-form-label">Code for header:</label>
                        <div class="col-sm-7">
                        <textarea class="form-control" id="site_analytic" name="site_analytic" rows="5">{{ $setting->site_analytic }}</textarea>
                        </div>
                    </div>
                </div>

                <div class="tab-pane" id="adsense" role="tabpanel">
                    <div class="form-group">
                        <label for="header_ads">Post Adsense code :</label>
                        <textarea class="form-control" id="post_ads" name="post_ads" rows="5">{{ $setting->post_ads }}</textarea>
                    </div>

                    <div class="form-group">
                        <label for="page_ads">Post Sidebar Adsense code :</label>
                        <textarea class="form-control" id="page_ads" name="page_ads" rows="5">{{ $setting->page_ads }}</textarea>
                    </div> 
                    <div class="form-group">
                        <label for="page_ads">Between Posts Adsense code :</label>
                        <textarea class="form-control" id="between_ads" name="between_ads" rows="5">{{ $setting->between_ads }}</textarea>
                    </div>                   
                </div>

                <div class="tab-pane" id="facebook" role="tabpanel">
                    <div class="form-group row">
                        <label for="fb_theme" class="col-sm-3 col-form-label">Theme</label>
                        <div class="col-sm-9">
                        <input type="text" class="form-control" id="fb_theme" name="fb_theme" value="{{ $setting->fb_theme }}">
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="fb_ads_code" class="col-sm-3 col-form-label">Ads code :</label>
                        <div class="col-sm-9">
                        <textarea class="form-control" id="fb_ads_code" name="fb_ads_code" rows="5">{{ $setting->fb_ads_code }}</textarea>
                        </div>
                    </div>
                </div>

                <div class="tab-pane" id="google" role="tabpanel">
                    <div class="form-group row">
                        <label for="amp_ad_server" class="col-sm-3 col-form-label">Ads on Amp?</label>
                        <div class="col-sm-9">
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input" 
                                    @if ($setting->amp_ad_server == '0')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="amp_ad_server" id="inlineRadio1" value="0"> No 
                                </label>
                            </div>
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input"
                                    @if ($setting->amp_ad_server == '1')
                                    checked="checked" 
                                    @endif
                                    type="radio" name="amp_ad_server" id="inlineRadio2" value="1"> Yes
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="amp_adscode" class="col-sm-3 col-form-label">Ads Code</label>
                        <div class="col-sm-9">
                        <textarea class="form-control" id="amp_adscode" name="amp_adscode" rows="5">{{ $setting->amp_adscode }}</textarea>
                        </div>
                    </div>
                </div>            
                </div>

                <div class="form-group row">
                    <div class="offset-sm-4 col-sm-7">
                        <button type="submit" class="btn btn-primary">Save</button>
                        <a href="{{ url('/home/') }}" class="btn btn-danger" role="button">Cancel</a> 
                    </div>
                </div>
                
            </div>
        </form>
    </div>
</div>
@endsection