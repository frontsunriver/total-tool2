@extends('theme::layouts.app')

@section('content')
<!-- contact area start -->
<section id="contact">
    <div class="contact-area pt-100 pb-100">
        <div class="container">
            <div class="row">
                <div class="col-lg-6 offset-lg-3">
                    <div class="section-header-title text-center pb-30">
                        <h2 id="contact_title">{{ __('Contact Us') }}</h2>
                       
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="custom-form">
                        <form action="{{ route('contact.send') }}" method="POST" id="contact_form">
                        @csrf                          <div class="row">
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="name" placeholder="{{ __('Name') }}" name="name">
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <input type="email" class="form-control" id="email" placeholder="{{ __('Email') }}" name="email">
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="subject" placeholder="{{ __('Subject') }}" name="subject">
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <textarea name="message" id="message" class="form-control" cols="30" rows="6" placeholder="{{ __('Message') }}"></textarea>
                                    </div>
                                </div>
                                <div class="col-lg-8 text-center">
                                    <h5 class="text-success pt-15" id="success"></h5>
                                    <h5 class="text-danger" id="error"></h5>
                                </div>
                                <div class="col-lg-4">
                                    <button type="submit" class="btn-submit f-right" id="contact_button_label">{{ __('Send Message') }}</button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<!-- contact area end -->
@endsection