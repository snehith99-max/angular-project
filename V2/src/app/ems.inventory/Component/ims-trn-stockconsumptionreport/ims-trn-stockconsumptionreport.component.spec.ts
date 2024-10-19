import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStockconsumptionreportComponent } from './ims-trn-stockconsumptionreport.component';

describe('ImsTrnStockconsumptionreportComponent', () => {
  let component: ImsTrnStockconsumptionreportComponent;
  let fixture: ComponentFixture<ImsTrnStockconsumptionreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStockconsumptionreportComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStockconsumptionreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
