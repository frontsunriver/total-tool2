@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-12">
				<h4>{{ __('Reply Comment') }}</h4>

				<p>{{ __('Name:') }} {{ $comments->name }}</p>
				<p>{{ __('Email:') }} {{ $comments->email }}</p>
			</div>
		</div>
		<div class="row">
			<div class="col-lg-12">
				<div class="comment-reply-box">
					<div class="comment-chat">
						<section class="discussion">
							<div class="bubble sender">{{ $comments->comment }} </div>
							@foreach($comments->reply as $row)
								<div class="bubble recipient">{{ $row->comment }}</div>
							@endforeach
						</section>		
					</div>
				</div>
				<div class="write-comment">
					<form method="post" id="basicform" action="{{ route('admin.comment.store') }}">
					@csrf
					<input type="hidden" name="id" value="{{ $comments->id }}">
					<input type="hidden" name="post_id" value="{{ $comments->term_id }}">
					<input type="text" name="reply" id="reply_input" placeholder="Reply Comment">
					<button type="submit"><i class="far fa-paper-plane"></i></button>
					</form>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//success response will assign this function
	function success(res){
		location.reload();
	}
</script>
@endsection