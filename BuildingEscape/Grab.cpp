// Fill out your copyright notice in the Description page of Project Settings.

#include "DrawDebugHelpers.h"
#include "Engine/World.h"
#include "GameFramework/PlayerController.h"

#include "Grab.h"

#define OUT

// Sets default values for this component's properties
UGrab::UGrab()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UGrab::BeginPlay()
{
	Super::BeginPlay();
	
	FindPhysicsHandle();
	SetUpInputComponent();
}

void UGrab::FindPhysicsHandle()
{
	PhysicsHandle = GetOwner()->FindComponentByClass<UPhysicsHandleComponent>();
	if(!PhysicsHandle)//Protection against nullptr
	{
		UE_LOG(LogTemp, Error, TEXT("No physics handle component found on %s."),
			*GetOwner()->GetName());
	}
}

void UGrab::SetUpInputComponent()
{
	InputComponent = GetOwner()->FindComponentByClass<UInputComponent>();
	if (InputComponent)
	{
		UE_LOG(LogTemp, Warning, TEXT("Input component found on: %s"), 
			*GetOwner()->GetName());
		InputComponent->BindAction("PickUp", IE_Pressed, this, &UGrab::PickUp);
		InputComponent->BindAction("PickUp", IE_Released, this, &UGrab::Drop);
	}
}

void UGrab::PickUp()
{
	UE_LOG(LogTemp, Warning, TEXT("PickUp Pressed"));

	//Only raycast when player wants
	FHitResult HitResult = GetFirstPhysicsBodyInReach();
	UPrimitiveComponent* ComponentToGrab = HitResult.GetComponent();
	AActor* ActorHit = HitResult.GetActor();

	if (ActorHit)
	{	if (!PhysicsHandle) {return;}
		PhysicsHandle->GrabComponentAtLocation
		(
			ComponentToGrab,
			NAME_None,
			GetPlayerReach()
		);
	}
}

void UGrab::Drop()
{
	UE_LOG(LogTemp, Warning, TEXT("PickUp Released"));
	//Remove physics handle
	PhysicsHandle->ReleaseComponent();
}

// Called every frame
void UGrab::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	//If physics handle is attached
	if (PhysicsHandle->GrabbedComponent)
	{	if (!PhysicsHandle) {return;}
		//Move the object grabbed
		PhysicsHandle->SetTargetLocation(GetPlayerReach());
	}
	// DrawDebugLine(
	// 	GetWorld(),
	// 	GetPlayerWorldPostion(),
	// 	GetPlayerReach(),
	// 	FColor(0, 0, 255),
	// 	false,
	// 	0.f,
	// 	0,
	// 	5.f
	// );
}

FHitResult UGrab::GetFirstPhysicsBodyInReach() const
{

	//Ray cast out to a set distance
	FHitResult Hit;
	FCollisionQueryParams TraceParams(FName(TEXT("")), false, GetOwner());
	GetWorld()->LineTraceSingleByObjectType(
		OUT Hit,
		GetPlayerWorldPostion(),
		GetPlayerReach(),
		FCollisionObjectQueryParams(ECollisionChannel::ECC_PhysicsBody),
		TraceParams
	);

	if (Hit.GetActor())
	{
		UE_LOG(LogTemp, Warning, TEXT("Object %s was hit."), 
			*Hit.GetActor()->GetName());
	}
	return Hit;
}

FVector UGrab::GetPlayerDirectionFacing() const
{
	FVector PlayerViewLocation;
	FRotator PlayerViewRotation;

	//Get player viewpoint
	GetWorld()->GetFirstPlayerController()->GetPlayerViewPoint(
		OUT PlayerViewLocation, 
		OUT PlayerViewRotation);
	
	return PlayerViewRotation.Vector();
}

FVector UGrab::GetPlayerReach() const
{
	FVector PlayerViewLocation;
	FRotator PlayerViewRotation;

	//Get player viewpoint
	GetWorld()->GetFirstPlayerController()->GetPlayerViewPoint(
		OUT PlayerViewLocation, 
		OUT PlayerViewRotation);
	
	FVector LineTraceDirection = PlayerViewRotation.Vector();
	return PlayerViewLocation + LineTraceDirection * ArmLength;
}

FVector UGrab::GetPlayerWorldPostion() const
{
	FVector PlayerViewLocation;
	FRotator PlayerViewRotation;

	//Get player viewpoint
	GetWorld()->GetFirstPlayerController()->GetPlayerViewPoint(
		OUT PlayerViewLocation, 
		OUT PlayerViewRotation);

	return PlayerViewLocation;
}