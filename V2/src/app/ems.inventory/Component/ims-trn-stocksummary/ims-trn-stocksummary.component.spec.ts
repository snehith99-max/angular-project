import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocksummaryComponent } from './ims-trn-stocksummary.component';

describe('ImsTrnStocksummaryComponent', () => {
  let component: ImsTrnStocksummaryComponent;
  let fixture: ComponentFixture<ImsTrnStocksummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocksummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocksummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
