import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstUserviewComponent } from './otl-mst-userview.component';

describe('OtlMstUserviewComponent', () => {
  let component: OtlMstUserviewComponent;
  let fixture: ComponentFixture<OtlMstUserviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstUserviewComponent]
    });
    fixture = TestBed.createComponent(OtlMstUserviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
