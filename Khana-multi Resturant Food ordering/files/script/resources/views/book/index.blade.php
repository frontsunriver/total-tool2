@component('mail::message')
# Hi {{ $user->name }}

You got a booking Request from {{ $data['name'] }}. Here is booking info:

@component('mail::table')
| Name       			| Email         		| Phone  |
| --------------------- |:--------------------:	| --------:|
| {{ $data['name'] }} 	| {{ $data['email'] }}  | {{ $data['mobile'] }} |
@endcomponent

Booking Date : {{ $data['date'] }}<br>
Total Gutes	 : {{ $data['number_of_gutes'] }}<br>
Message 	 : {{ $data['message'] }}

Thanks,<br>
{{ config('app.name') }}
@endcomponent
