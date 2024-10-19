import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferacknowledgementSummaryComponent } from './ims-trn-stocktransferacknowledgement-summary.component';

describe('ImsTrnStocktransferacknowledgementSummaryComponent', () => {
  let component: ImsTrnStocktransferacknowledgementSummaryComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferacknowledgementSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferacknowledgementSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferacknowledgementSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
