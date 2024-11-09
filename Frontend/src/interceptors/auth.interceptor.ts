import { HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from "@angular/core";
import { isPlatformServer } from "@angular/common";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  if (isPlatformServer(inject(PLATFORM_ID))) {
    return next(req);
  }

  const accessToken = localStorage.getItem('accessToken');

  if (accessToken === null)
    return next(req);

  const newReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${accessToken}`)
  });

  return next(newReq);
};
