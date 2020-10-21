# CarRentalAdministrationService
C# ASP.NET Core application which exposes REST-API endpoints for a car rental administration service system.

Applikationen triggas vias rest anrop:

Exempel use case för att skapa och stänga en order:

för att skapa ny order:
POST: https://localhost:44379/api/orders
{
  "Created": "2020-10-20",
  "CarCategory": "Premium",
  "CustomerDateOfBirth": "1987-01-12"
}

Resultat blir ett json som beskriver objektet som har skapats:

{
  "orderId": [X],
  "created": "2020-10-20T00:00:00",
  "closed": "0001-01-01T00:00:00",
  "customerDateOfBirth": "1987-01-12T00:00:00",
  "car": {
    "carId": 3,
    "name": "BatMobile",
    "carCategory": {
      "carCategoryId": 2,
      "category": "Premium",
      "baseDayRentalCost": 125,
      "kilometerPrice": 12
    },
    "mileageInKilometers": 250,
    "available": false
  }
}

För att lämna in bilen och avsluta ordren så kan et patch anrop göras på samma endpoint der man anger samma ordreId som man fekk som svar på post anropet
Passa på att ange en dato och mileage som är senare och större än hyrdatot respektiva mileage på bilen annars kommer et 404 med ett meddelandet att returneras.
PATCH https://localhost:44379/api/orders
{
  "BookingNr": [X],
  "ReturnDate": "2020-10-22",
 "MileageInKm": 275
}

Om allting har gått ok så får du kostnaden tillskickat och ordren blir stängd.
{
  "cost": 600
}

