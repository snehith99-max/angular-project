import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnTradedaytrackerComponent } from './otl-trn-tradedaytracker.component';

describe('OtlTrnTradedaytrackerComponent', () => {
  let component: OtlTrnTradedaytrackerComponent;
  let fixture: ComponentFixture<OtlTrnTradedaytrackerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnTradedaytrackerComponent]
    });
    fixture = TestBed.createComponent(OtlTrnTradedaytrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
