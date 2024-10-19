import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferSummaryComponent } from './ims-trn-stocktransfer-summary.component';

describe('ImsTrnStocktransferSummaryComponent', () => {
  let component: ImsTrnStocktransferSummaryComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
