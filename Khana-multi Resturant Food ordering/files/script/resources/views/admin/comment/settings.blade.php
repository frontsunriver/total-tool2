@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-12">
				<h4>{{ __('Third Party API Settings') }}</h4>
			</div>
		</div>
		
		<div class="card-action-filter">
			<form method="post" action="{{ route('admin.disqus.store') }}" id="basicform">
				@csrf
				<div class="form-group">
					<label><a href="https://disqus.com/admin/create/" target="_blank">{{ __('Disqus') }}</a> {{ __('Comment API Source') }} </label><br>
					<small>{{ __('Your unique') }} <a href="https://disqus.com/admin/create/" target="_blank">{{ __('disqus') }}</a> {{ __('URL will be: shortname.disqus.com') }}</small>
					<input type="text" name="disqus" class="form-control" placeholder="Enter your unique url" required="" value="{{ \App\Options::where('key','disqus_comment')->first()->value }}">
				</div>
				<div class="form-group">
					<button class="btn" type="submit">{{ __('Submit') }}</button>
				</div>
			</form>
		</div>
	</div>
</div>

@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@endsection