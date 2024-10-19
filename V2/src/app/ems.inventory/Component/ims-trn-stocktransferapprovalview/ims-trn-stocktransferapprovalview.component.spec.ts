import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferapprovalviewComponent } from './ims-trn-stocktransferapprovalview.component';

describe('ImsTrnStocktransferapprovalviewComponent', () => {
  let component: ImsTrnStocktransferapprovalviewComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferapprovalviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferapprovalviewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferapprovalviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
