import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferapprovalSummaryComponent } from './ims-trn-stocktransferapproval-summary.component';

describe('ImsTrnStocktransferapprovalSummaryComponent', () => {
  let component: ImsTrnStocktransferapprovalSummaryComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferapprovalSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferapprovalSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferapprovalSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
