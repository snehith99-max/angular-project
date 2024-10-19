import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStockdamageComponent } from './ims-trn-stockdamage.component';

describe('ImsTrnStockdamageComponent', () => {
  let component: ImsTrnStockdamageComponent;
  let fixture: ComponentFixture<ImsTrnStockdamageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStockdamageComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStockdamageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
