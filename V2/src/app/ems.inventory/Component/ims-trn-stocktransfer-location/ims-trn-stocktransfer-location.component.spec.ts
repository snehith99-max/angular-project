import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferLocationComponent } from './ims-trn-stocktransfer-location.component';

describe('ImsTrnStocktransferLocationComponent', () => {
  let component: ImsTrnStocktransferLocationComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferLocationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferLocationComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
