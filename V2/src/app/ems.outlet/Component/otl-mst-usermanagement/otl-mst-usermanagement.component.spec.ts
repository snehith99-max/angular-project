import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstUsermanagementComponent } from './otl-mst-usermanagement.component';

describe('OtlMstUsermanagementComponent', () => {
  let component: OtlMstUsermanagementComponent;
  let fixture: ComponentFixture<OtlMstUsermanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstUsermanagementComponent]
    });
    fixture = TestBed.createComponent(OtlMstUsermanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
