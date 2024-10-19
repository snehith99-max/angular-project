import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnOpendcComponent } from './ims-trn-opendc.component';

describe('ImsTrnOpendcComponent', () => {
  let component: ImsTrnOpendcComponent;
  let fixture: ComponentFixture<ImsTrnOpendcComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnOpendcComponent]
    });
    fixture = TestBed.createComponent(ImsTrnOpendcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
