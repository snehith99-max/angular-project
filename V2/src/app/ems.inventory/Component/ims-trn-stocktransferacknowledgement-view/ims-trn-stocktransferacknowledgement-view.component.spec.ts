import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferacknowledgementViewComponent } from './ims-trn-stocktransferacknowledgement-view.component';

describe('ImsTrnStocktransferacknowledgementViewComponent', () => {
  let component: ImsTrnStocktransferacknowledgementViewComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferacknowledgementViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferacknowledgementViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferacknowledgementViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
