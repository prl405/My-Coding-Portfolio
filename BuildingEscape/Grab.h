// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "PhysicsEngine/PhysicsHandleComponent.h"
#include "Grab.generated.h"


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class BUILDINGESCAPE_API UGrab : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UGrab();

	UPROPERTY(EditAnywhere)
	float ArmLength = 200.f;

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

private:

	UPROPERTY()
	UPhysicsHandleComponent* PhysicsHandle = nullptr;
	UPROPERTY()
	UInputComponent* InputComponent = nullptr;

	void PickUp();
	void Drop();
	void FindPhysicsHandle();
	void SetUpInputComponent();

	FHitResult GetFirstPhysicsBodyInReach() const;

	FVector GetPlayerReach() const;
	FVector GetPlayerDirectionFacing() const;
	FVector GetPlayerWorldPostion() const;
};
