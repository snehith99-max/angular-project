import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktrnasferbranchViewComponent } from './ims-trn-stocktrnasferbranch-view.component';

describe('ImsTrnStocktrnasferbranchViewComponent', () => {
  let component: ImsTrnStocktrnasferbranchViewComponent;
  let fixture: ComponentFixture<ImsTrnStocktrnasferbranchViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktrnasferbranchViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktrnasferbranchViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
