import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnAddstockComponent } from './ims-trn-addstock.component';

describe('ImsTrnAddstockComponent', () => {
  let component: ImsTrnAddstockComponent;
  let fixture: ComponentFixture<ImsTrnAddstockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnAddstockComponent]
    });
    fixture = TestBed.createComponent(ImsTrnAddstockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
